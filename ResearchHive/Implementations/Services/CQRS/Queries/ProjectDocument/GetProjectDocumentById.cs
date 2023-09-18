using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.ProjectDocument;

public class GetProjectDocumentById
{
    public class ProjectDocumentDTO
    {
        public Guid Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Folder { get; set; }
        public string DocumentPath { get; set; }

        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; }

    }
    public record Request() : IQuery<ProjectDocumentDTO>
    {
        [JsonIgnore] public Guid Id { get; set; }
    }
    public record Handler : IQueryHandler<Request, ProjectDocumentDTO>
    {
        readonly IProjectDocumentRepository _projectDocumentRepository;

        public Handler(IProjectDocumentRepository projectDocumentRepository) =>
        (_projectDocumentRepository) = (projectDocumentRepository);

        public async Task<Result<ProjectDocumentDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var projectDocument = await _projectDocumentRepository.GetProjectDocumentAsync(request.Id);
            if (projectDocument is null) return await Result<ProjectDocumentDTO>.FailAsync($"project document with id: {request.Id} not found");
            var projectDocumentDTO = new ProjectDocumentDTO
            {
                Id = projectDocument.Id,
                ChapterNumber = projectDocument.ChapterNumber,
                ProjectId = projectDocument.ProjectId,
                DocumentPath = projectDocument.DocumentPath,
                Folder = projectDocument.Folder,
                ProjectTitle = projectDocument.Project.Title,
            };
            return await Result<ProjectDocumentDTO>.SuccessAsync(projectDocumentDTO, $"project document with id: {request.Id} retrieval successful");

        }
    }
}
