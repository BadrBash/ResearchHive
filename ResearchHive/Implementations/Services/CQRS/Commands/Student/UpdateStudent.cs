
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.student;

public class UpdateStudent
{
    public class RequestDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
    public record Request(RequestDto Model) : IQuery<Guid>
    {
       [JsonIgnore] public Guid UpdatedBy { get; set; }
       [JsonIgnore] public Guid Id { get; set; }
    }

    public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;

        public Handler(IStudentRepository studentRepository, IUserRepository userRepository) =>
        (_studentRepository, _userRepository) = (studentRepository, userRepository);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByExpressionAsync(usr => usr.Id == request.Id);
            if (student is null) return await Result<Guid>.FailAsync($"student with id: {request.Id} not found");
            var studentUserId = student.UserId;
            var user = await _userRepository.GetAsync(studentUserId);
            user.UpdatedBy = request.UpdatedBy;
            var updatedUser =  user?.Update(request.Model.FirstName, request.Model.LastName, request.Model.Email, request.Model.PhoneNumber);
            var updateRequest = await _userRepository.UpdateAsync(updatedUser);
            var saveChangesResult = await _userRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for student with id: {request.Id}");
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
        }
    }
}
