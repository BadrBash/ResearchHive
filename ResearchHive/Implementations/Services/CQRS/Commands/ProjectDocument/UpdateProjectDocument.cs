
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.ProjectDocument;

public class UpdateProjectDocument
{
   
    public record Request() : IQuery<Guid>
    {
        [JsonIgnore] public Guid Id { get; set; }
        [JsonIgnore] public Guid UpdatedBy { get; set; }
    }


    public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly IFileService _fileService;
        private readonly IProjectDocumentRepository _ProjectDocumentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Handler(IProjectDocumentRepository ProjectDocumentRepository, IFileService fileService, IWebHostEnvironment webHostEnvironment) =>
        (_ProjectDocumentRepository, _fileService, _webHostEnvironment) = (ProjectDocumentRepository, fileService, webHostEnvironment);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var ProjectDocument = await _ProjectDocumentRepository.GetProjectDocumentAsync(usr => usr.Id == request.Id);
            if (ProjectDocument is null) return await Result<Guid>.FailAsync($"ProjectDocument with id: {request.Id} not found");
            var deleteResponse = IFileService.DeleteFile(ProjectDocument.Folder, ProjectDocument.DocumentPath);
            if(deleteResponse)
            {
                var fileUploadResponse = await _fileService.UploadProjectsAsync(new ProjectDocumentUploadModel
                {
                    Author = $"{ProjectDocument.Project?.Student?.User?.FirstName} {ProjectDocument.Project?.Student?.User?.LastName}",
                    MaxNumberOfChapters = ProjectDocument.Project.ProjectSubmissionWindow.NumberOfChapters,
                    ProjectTitle = ProjectDocument?.Project?.Title
                });
                var updatedProjectDocument = ProjectDocument?.Update(fileUploadResponse.DocumentPathsAndNumber[0].DocumentPath);
                updatedProjectDocument.UpdatedBy = request.UpdatedBy;
                var updateRequest = await _ProjectDocumentRepository.UpdateAsync(updatedProjectDocument);
                var saveChangesResult = await _ProjectDocumentRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for ProjectDocument with id: {request.Id}");
                return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
            }
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
           


        }
    }
}
