
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Lecturer;

public class UpdateLecturer
{
    public class RequestDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public Guid UpdatedBy { get; set; }
    }
    public record Request(Guid Id, RequestDto Model) : IQuery<Guid>;

    public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly ILecturerRepository _LecturerRepository;
        private readonly IUserRepository _userRepository;

        public Handler(ILecturerRepository LecturerRepository, IUserRepository userRepository) =>
        (_LecturerRepository, _userRepository) = (LecturerRepository, userRepository);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Lecturer = await _LecturerRepository.GetAsync(usr => usr.Id == request.Id);
            if (Lecturer is null) return await Result<Guid>.FailAsync($"Lecturer with id: {request.Id} not found");
            var LecturerUserId = Lecturer.UserId;
            var user = await _userRepository.GetAsync(LecturerUserId);
            var updatedUser =  user?.Update(request.Model.FirstName, request.Model.LastName, request.Model.Email, request.Model.PhoneNumber);
            Lecturer.UpdatedBy = request.Model.UpdatedBy;
            var updateRequest = await _userRepository.UpdateAsync(updatedUser);
            var saveChangesResult = await _userRepository.SaveChangesAsync(cancellationToken);
            var saveChangesResultLecturer = await _LecturerRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1 && saveChangesResultLecturer == 1)  return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for Lecturer with id: {request.Id}");
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
           


        }
    }
}
