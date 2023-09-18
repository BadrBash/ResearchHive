using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Implementations.Services.CQRS.Queries.ProjectDocument
{
    public class GetProjectDocumentsByStudentId
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
        public record Request() : IQuery<IEnumerable<ProjectDocumentDTO>>
        {
            [JsonIgnore] public Guid StudentId { get; set; }
        }
        public record Handler : IQueryHandler<Request, IEnumerable<ProjectDocumentDTO>>
        {
            readonly IProjectDocumentRepository _projectDocumentRepository;

            public Handler(IProjectDocumentRepository projectDocumentRepository) =>
            (_projectDocumentRepository) = (projectDocumentRepository);

            public async Task<Result<IEnumerable<ProjectDocumentDTO>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var projectDocuments = await _projectDocumentRepository.GetProjectDocumentsAsync(prd => prd.Project.Student.UserId == request.StudentId && !prd.IsDeleted);
                if (projectDocuments.Count() <= 0) return await Result<IEnumerable<ProjectDocumentDTO>>.FailAsync("project Documents' retrieval returned empty data");
                var projectDocumentsDTO = projectDocuments.Select(proj => new ProjectDocumentDTO
                {
                    Id = proj.Id,
                    ChapterNumber = proj.ChapterNumber,
                    ProjectId = proj.ProjectId,
                    DocumentPath = proj.DocumentPath,
                    Folder = proj.Folder,
                    ProjectTitle = proj.Project.Title,
                });
                return await Result<IEnumerable<ProjectDocumentDTO>>.SuccessAsync(projectDocumentsDTO, "project Documents' retrieval successful");

            }
        }
    }
}
