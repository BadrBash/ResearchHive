
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Persistence.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Student;

public class GetByUserId
{

    public record Request(Guid userId) : IQuery<StudentDTO>;

    public record Handler : IQueryHandler<Request, StudentDTO>
    {
        private readonly IStudentRepository _studentRepository;

        public Handler(IStudentRepository studentRepository) =>
        (_studentRepository) = (studentRepository);

        public async Task<Result<StudentDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByExpressionAsync(std => std.UserId ==  request.userId);
            if (student is null) return await Result<StudentDTO>.FailAsync($"student with user id: {request.userId} not found");
            var studentReturned = new StudentDTO
            {
                Email = student.User.Email,
                UserId = student.UserId,
                UserName = student.User.UserName,
                FirstName = student.User.FirstName,
                LastName = student.User.LastName,
                PhoneNumber = student.User.PhoneNumber,
                UserRoles = student.User.UserRoles.Select(us => us.Role.Name).ToList(),
                Department = student.Department.Name
            };
            return await Result<StudentDTO>.SuccessAsync(studentReturned, $"student with user id: {request.userId} successfully retrieved");


        }
    }
}
