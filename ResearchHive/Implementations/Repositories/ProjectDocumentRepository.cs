using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Persistence.Repositories
{
    public class ProjectDocumentRepository : BaseRepository<ProjectDocument>, IProjectDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectDocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<ProjectDocument>> GetPaginatedProjectDocumentsAsync(Guid projectId, PaginationFilter filter)
        {
           return await _context.ProjectDocuments.Include(pD => pD.Project).
                Where(pD => pD.ProjectId == projectId).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);  
        }

        public async Task<ProjectDocument> GetProjectDocumentAsync(Guid id)
        {
            return await _context.ProjectDocuments.Include(pD => pD.Project)
                .Where(pD => pD.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProjectDocument> GetProjectDocumentAsync(Expression<Func<ProjectDocument, bool>> expression)
        {
            return await _context.ProjectDocuments.Include(pD => pD.Project)
                .Where(expression).FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<ProjectDocument>> GetProjectDocumentsAsync(Guid projectId)
        {
            return await _context.ProjectDocuments.
                Include(pD => pD.Project)
                .Where(pD => pD.ProjectId == projectId).ToListAsync();
        }

        public async Task<PaginatedResult<ProjectDocument>> GetProjectDocumentsAsync(Expression<Func<ProjectDocument, bool>> expression, PaginationFilter filter)
        {
            return await _context.ProjectDocuments.
                Include(pD => pD.Project)
                .Where(expression).ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }

        public async Task<IEnumerable<ProjectDocument>> GetProjectDocumentsAsync()
        {
            return await _context.ProjectDocuments.
            Include(pD => pD.Project)
                 .ToListAsync();
        }

        public async Task<IEnumerable<ProjectDocument>> GetProjectDocumentsAsync(Expression<Func<ProjectDocument, bool>> expression)
        {

            return await _context.ProjectDocuments.Include(pD => pD.Project)
                .Where(expression).ToListAsync();
        }
    }
}
