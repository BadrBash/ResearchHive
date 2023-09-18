using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Department;

public class GetAll
{
    public class DepartmentDto
    {
      public string Name { get; set; }
      public Guid Id { get; set; }
        public string Description { get; set; } 
        public ICollection<StudentDTO> Students { get; set; } = new List<StudentDTO>();
        public ICollection<string> Lecturers { get; set; } = new List<string>();

    }
    public record Request() : IQuery<IEnumerable<DepartmentDto>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<DepartmentDto>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public Handler(IDepartmentRepository departmentRepository) =>
        (_departmentRepository) = (departmentRepository);

        public async Task<Result<IEnumerable<DepartmentDto>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var departments = await _departmentRepository.Query()
                .Include(de => de.Students).ThenInclude(st => st.User)
                .Include(de => de.LecturerDepartments)
                .ThenInclude(ld => ld.Lecturer).ThenInclude(st => st.User)
                .ToListAsync();
            var departmentsDTO = departments.Select(dept => new DepartmentDto
            {
                Id = dept.Id,
                Description = dept.Description,
                Name = dept.Name,
                Lecturers = dept.LecturerDepartments.Select(ld =>
                $"{ld.Lecturer.User.FirstName} {ld.Lecturer.User.LastName}")
                .ToList(),
                Students = dept.Students.Select(std => new StudentDTO
                {
                    ActiveStatus = std.IsActiveStudent ? "Active Student" : "Inactive Student",
                    Department = dept.Name,
                    FirstName = std.User.FirstName,
                    LastName = std.User.LastName,
                    Email = std.User.Email,
                    Level = std.Level.ToString(),
                    MatricNumber = std.MatricNumber,
                    PhoneNumber = std.User.PhoneNumber,
                    UserId = std.UserId,
                    UserName = std.User.UserName,
                    UserRoles = std.User.UserRoles.Select(role => role.Role.Name).ToList()
                }).ToList()
            });
            if(departments.Count() <= 0) return await  Result<IEnumerable<DepartmentDto>>.FailAsync("departments' retrieval returned empty data");
            return await  Result<IEnumerable<DepartmentDto>>.SuccessAsync(departmentsDTO, "Participants' retrieval successful");
           
        }
    }
}
