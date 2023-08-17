
using Application.DTOs;
using ResearchHive.Wrapper;
using Model.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ILecturerDepartmentRepository : IRepository<LecturerDepartment>
    {
        Task<PaginatedResult<LecturerDepartmentDto>> GetLecturerDepartmentsAsync(Guid lecturerId, PaginationFilter filter);
        Task<IReadOnlyList<LecturerDepartment>> GetAsync();
    }
}
