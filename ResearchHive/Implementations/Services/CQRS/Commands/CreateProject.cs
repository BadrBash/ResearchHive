using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Model.Entities;
using Model.Enums;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CreateProjectRequest
    {
        public record Request(string Title, string Description, string Summary, ProjectStage ProjectStage, int ChapterNumber) : ICommand<Guid>
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
                var projectExists = await _projectRepository.ExistsAsync(proj => (proj.Title == request.Title && proj.IsApproved) || (proj.ProjectSubmissionWindow.Level == studentCreating.Level) || proj.Title == request.Title);
                var projectSubmissionWindow = await _projectSubmissionWindowRepository.GetByExpressionAsync(psw => psw.DepartmentName ==
                studentCreating.Department.Name);
                if(projectExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }

                var uploadResponse = await _fileService.UploadProjectsAsync(new ProjectDocumentUploadModel()
                {
                    Author = request.CreatorName,
                    ProjectTitle = request.Title,
                }, projectSubmissionWindow.NumberOfChapters,cancellationToken);
                var project = new Project(request.Title, request.Description, request.Summary, 
                    studentCreating.Id, projectSubmissionWindow.Id, ProjectStage.Started);
                var projectUpload = await _projectRepository.AddAsync(project);
                var saveProject = await _projectDocumentRepository.SaveChangesAsync(cancellationToken);
                var chapterCount = project.ProjectDocuments.LastOrDefault()?.ChapterNumber;
                var successfulUploads = 0;
                foreach (var documentPathAndNumber in uploadResponse.DocumentPathsAndNumber)
                {
                    chapterCount++; //To get the chapter for the current document uploaded.
                   
                    if(projectSubmissionWindow.NumberOfChapters != chapterCount) // To know if the required amount of chapters to be submitted
                                                                                 // is already complete
                    {
                        var projectDocument = new ProjectDocument(documentPathAndNumber.ChapterNumber, projectUpload.Id, documentPathAndNumber.DocumentPath);
                        projectUpload.ProjectDocuments.Add(projectDocument);
                        await _projectDocumentRepository.AddAsync(projectDocument);
                        var saveResponse = await _projectDocumentRepository.SaveChangesAsync(cancellationToken);
                        chapterCount = project.ProjectDocuments.LastOrDefault()?.ChapterNumber;
                        if (saveResponse == 1)
                        {
                            successfulUploads++;
                        }
                    }
                }
                var completedUploadProject = projectUpload.CompleteSubmission();
                await _projectRepository.UpdateAsync(completedUploadProject);
                var saveResult = await _projectRepository.SaveChangesAsync(cancellationToken);
                if(saveResult == 1)
                {
                    return new Result<Guid>
                    {
                        Data = projectUpload.Id,
                        Messages = new List<string> {uploadResponse.Message,
                                $"{successfulUploads} Project(s) {ResponseMessage.UploadSuccessful} to the db.", ResponseMessage.AwaitingApproval},
                        Succeeded = true
                    };
                }
                    await _projectRepository.UpdateAsync(projectUpload);
                    await _projectRepository.SaveChangesAsync(cancellationToken);
                    return new Result<Guid> 
                    {
                        Data = projectUpload.Id,
                        Messages = new List<string> {uploadResponse.Message,
                                $"{successfulUploads} Project(s) {ResponseMessage.UploadSuccessful} to the db."},
                        Succeeded = true
                    };
                

            }
        }
    }
}
