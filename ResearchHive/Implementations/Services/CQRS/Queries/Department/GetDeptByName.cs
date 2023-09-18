using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.Department;

public class GetDeptByName
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<StudentDTO> Students { get; set; } = new List<StudentDTO>();
        public ICollection<string> Lecturers { get; set; } = new List<string>();
    }
    public record Request() : IQuery<DepartmentDto>
    {
        [JsonIgnore] public string Name { get; set; }
    }

    public record Handler : IQueryHandler<Request, DepartmentDto>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public Handler(IDepartmentRepository departmentRepository) =>
        (_departmentRepository) = (departmentRepository);

        public async Task<Result<DepartmentDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var dept = await _departmentRepository.GetAsync(dept => dept.Name == request.Name && dept.IsDeleted == false);
            if (dept is null) return await Result<DepartmentDto>.FailAsync($"department with name: {request.Name} not found");
            var departmentReturned = new DepartmentDto
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
            };
            return await Result<DepartmentDto>.SuccessAsync(departmentReturned, $"department with name: {request.Name} successfully retrieved");
            

        }
    }
}
