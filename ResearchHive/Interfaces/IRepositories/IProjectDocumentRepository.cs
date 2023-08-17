using Model.Entities;
using ResearchHive.Wrapper;
using System.Linq.Expressions;

namespace Application.Interfaces.Repositories
{
    public interface IProjectDocumentRepository : IRepository<ProjectDocument>
    {
        Task<IEnumerable<ProjectDocument>> GetProjectDocumentsAsync(Guid projectId);
        Task<PaginatedResult<ProjectDocument>> GetPaginatedProjectDocumentsAsync(Guid projectId, PaginationFilter filter);
        Task<ProjectDocument> GetProjectDocumentAsync(Guid id);
        Task<ProjectDocument> GetProjectDocumentAsync(Expression<Func<ProjectDocument, bool>> expression);
        Task<PaginatedResult<ProjectDocument>> GetProjectDocumentsAsync(Expression<Func<ProjectDocument, bool>> expression, PaginationFilter filter);

    }
}
