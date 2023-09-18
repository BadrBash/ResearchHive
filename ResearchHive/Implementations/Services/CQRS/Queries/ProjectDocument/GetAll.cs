using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Model.Enums;
using Persistence.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.ProjectDocument;

public class GetAllProjectDocuments
{ 
    public class ProjectDocumentDTO
    {
        public Guid Id { get; set;}
        public int ChapterNumber { get;  set; }
        public string Folder { get;  set; }
        public string DocumentPath { get;  set; }

        public Guid ProjectId { get;  set; }
        public string ProjectTitle { get;  set; }

    }
    public record Request() : IQuery<IEnumerable<ProjectDocumentDTO>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<ProjectDocumentDTO>>
    {
         readonly IProjectDocumentRepository _projectDocumentRepository;

        public Handler(IProjectDocumentRepository projectDocumentRepository) =>
        (_projectDocumentRepository) = (projectDocumentRepository);

        public async Task<Result<IEnumerable<ProjectDocumentDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var projectDocuments = await _projectDocumentRepository.GetProjectDocumentsAsync();
            if(projectDocuments.Count() <= 0) return await  Result<IEnumerable<ProjectDocumentDTO>>.FailAsync("project Documents' retrieval returned empty data");
            var projectDocumentsDTO = projectDocuments.Select(proj => new ProjectDocumentDTO
            {
                Id = proj.Id,   
               ChapterNumber   = proj.ChapterNumber,
               ProjectId = proj.ProjectId,
               DocumentPath = proj.DocumentPath,
               Folder = proj.Folder,
               ProjectTitle = proj.Project.Title,
            }).ToList();
            return await  Result<IEnumerable<ProjectDocumentDTO>>.SuccessAsync(projectDocumentsDTO, "project Documents' retrieval successful");
           
        }
    }
}
