using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories
{
    public class LecturerDepartmentRepository : BaseRepository<LecturerDepartment>, ILecturerDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public LecturerDepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<LecturerDepartment>> GetAsync()
        {
            return await _context.LecturerDepartments.ToListAsync();
        }

        public async Task<PaginatedResult<LecturerDepartmentDto>> GetLecturerDepartmentsAsync(Guid lecturerId, PaginationFilter filter)
        {
            return await _context.LecturerDepartments.Include(lD => lD.Department).Include(lD => lD.Lecturer)
                .ThenInclude(lD => lD.User)
                .Where(lD => lD.Lecturer.UserId == lecturerId)
                .Select(lD => new  LecturerDepartmentDto
                {
                    DepartmentName = lD.Department.Name,
                    Description = lD.Department.Description
                })
                .ToPaginatedResultListAsync(filter.PageNumber, filter.PageSize);
        }
    }
}
