using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Student;

public class GetAllStudents
{
   
    public record Request() : IQuery<IEnumerable<StudentDTO>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<StudentDTO>>
    {
        private readonly IStudentRepository _studentRepository;

        public Handler(IStudentRepository studentRepository) =>
        (_studentRepository) = (studentRepository);

        public async Task<Result<IEnumerable<StudentDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.Query()
                .Include(l => l.User)
                .Include(st => st.Department)
                .ToListAsync();
            var studentsDTO = students.Select(student => new StudentDTO
            {
                Email = student.User.Email,
                UserId = student.UserId,
                Department = student.Department.Name,
                UserName = student.User.UserName,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                PhoneNumber = student.User.PhoneNumber,
                UserRoles = student.User.UserRoles.Select(us => us.Role.Name).ToList()
            });
            if(students.Count() <= 0) return await  Result<IEnumerable<StudentDTO>>.FailAsync("students' retrieval returned empty data");
            return await  Result<IEnumerable<StudentDTO>>.SuccessAsync(studentsDTO, "students' retrieval successful");
           
        }
    }
}
