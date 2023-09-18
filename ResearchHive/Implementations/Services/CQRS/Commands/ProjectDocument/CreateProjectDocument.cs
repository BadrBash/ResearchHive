using Application.Abstractions.Messaging;
using Model.Constants;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Wrapper;
using Model.Entities;
using Model.Enums;
using Model.Extensions;
using ResearchHive.Abstractions.Messaging;
using System.Text.Json.Serialization;
using ResearchHive.Constants;

namespace ResearchHive.Implementations.Services
{
    public class CreateProjectDocumentRequest
    {
        public record Request(Guid ProjectId, int ChapterNumber) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
            [JsonIgnore]
            public string CreatorName { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {

            private readonly IProjectRepository _projectRepository;
            private readonly IProjectDocumentRepository _projectDocumentRepository;
            private readonly IFileService _fileService;
            private readonly IStudentRepository _studentRepository;
            private readonly IProjectSubmissionWindowRepository _projectSubmissionWindowRepository;
            public Handler(IProjectRepository projectRepository, IFileService fileService,
                IStudentRepository studentRepository, IProjectDocumentRepository projectDocumentRepository, IProjectSubmissionWindowRepository pswRepository)
            {
                _projectRepository = projectRepository;
                _fileService = fileService;
                _projectDocumentRepository = projectDocumentRepository;
                _studentRepository = studentRepository;
                _projectSubmissionWindowRepository = pswRepository;
            }
            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var studentCreating = await _studentRepository.GetByExpressionAsync(st => st.UserId == request.CreatedBy);
                var project = await _projectRepository.GetByExpressionAsync(proj => proj.Id == request.ProjectId);
                if (project.IsComplete)
                {
                    return new Result<Guid>
                    {
                        Messages = new List<string> { ResponseMessage.OperationFailed, $"Project: {project.Title} is already completed and is at {project.ProjectStage.GetDescription()} stage" },
                        Succeeded = false
                    };
                }

                if (project.ProjectStage == ProjectStage.Started || project.ProjectStage == ProjectStage.InProgress)
                {
                    var projectDocumentExists = await _projectDocumentRepository.ExistsAsync(pd => pd.ChapterNumber == request.ChapterNumber && pd.Project.StudentId == studentCreating.Id);
                    if (projectDocumentExists)
                    {
                        return new Result<Guid>
                        {
                            Messages = new List<string> { $"{ResponseMessage.RecordExist} for Chapter {request.ChapterNumber}" }
                        };

                    }

                    var projectDocumentsUpload = await _fileService.UploadProjectsAsync(new ProjectDocumentUploadModel
                    {
                        Author = request.CreatorName,
                        ProjectTitle = project.Title,
                        MaxNumberOfChapters = project.ProjectSubmissionWindow.NumberOfChapters,
                    }, cancellationToken);
                    var chapterCount = project.ProjectDocuments.LastOrDefault()?.ChapterNumber;
                    var successfulUploads = 0;
                    foreach (var documentPathAndNumber in projectDocumentsUpload.DocumentPathsAndNumber)
                    {
                        chapterCount++; //To get the chapter for the current document uploaded.

                        if (project.ProjectSubmissionWindow.NumberOfChapters != chapterCount) // To know if the required amount of chapters to be submitted
                                                                                              // is already complete
                        {
                            var projectDocument = new ProjectDocument(documentPathAndNumber.ChapterNumber, project.Id, documentPathAndNumber.DocumentPath, documentPathAndNumber.Folder);
                            project.ProjectDocuments.Add(projectDocument);
                            var saveResponse = await _projectDocumentRepository.SaveChangesAsync(cancellationToken);
                            if (saveResponse == 1)
                            {
                                successfulUploads++;
                            }
                        }

                    }

                    var completedUploadProject = project.CompleteSubmission();
                    await _projectRepository.UpdateAsync(completedUploadProject);
                    var saveResult = await _projectRepository.SaveChangesAsync(cancellationToken);
                    if (saveResult == 1)
                    {
                        return new Result<Guid>
                        {
                            Data = project.Id,
                            Messages = new List<string> {projectDocumentsUpload.Message,
                                $"{successfulUploads} Project(s) {ResponseMessage.UploadSuccessful} to the db.", ResponseMessage.AwaitingApproval},
                            Succeeded = true
                        };

                    }

                    return new Result<Guid>
                    {
                        Messages = new List<string>
                        {
                                $"{ResponseMessage.OperationFailed}" },
                        Succeeded = false
                    };

                }

                return new Result<Guid>
                {
                    Messages = new List<string>
                    {
                                $"Cannot Submit a document for project at {project.ProjectStage.GetDescription()}" },
                    Succeeded = false
                };


            }
        }
    }
}
