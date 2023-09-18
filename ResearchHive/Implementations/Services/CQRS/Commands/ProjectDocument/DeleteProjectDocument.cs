
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.ProjectDocument;

public class DeleteProjectDocument
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IProjectDocumentRepository _ProjectDocumentRepository;
        public Handler(IProjectDocumentRepository ProjectDocumentRepository) =>
        (_ProjectDocumentRepository) = (ProjectDocumentRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var ProjectDocument = await _ProjectDocumentRepository.GetProjectDocumentAsync(pd => pd.Id == request.Id);

            if (ProjectDocument is null) return await Result<string>.FailAsync($"Project Document with id: {request.Id} not found");
            ProjectDocument.IsDeleted = true;
            var deleteResponse = IFileService.DeleteFile(ProjectDocument.Folder, ProjectDocument.DocumentPath);
            if(deleteResponse)
            {
                _ProjectDocumentRepository.Remove(ProjectDocument);
                var saveChangesResult = await _ProjectDocumentRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for ProjectDocument with id: {request.Id}");
                return await Result<string>.FailAsync(ResponseMessage.OperationFailed);
            }
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed + "File cannot be accessed");
        }
    }
}
