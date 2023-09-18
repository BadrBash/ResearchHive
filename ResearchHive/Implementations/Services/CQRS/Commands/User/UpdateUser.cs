
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Admin;

public class UpdateUser
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
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository) =>
        (_userRepository) = (userRepository);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync( usr => usr.Id == request.Id);
            if (user is null) return await Result<Guid>.FailAsync($"user with id: {request.Id} not found");
          
            var updatedUser =  user?.Update(request.Model.FirstName, request.Model.LastName, request.Model.Email, request.Model.PhoneNumber);
            updatedUser.UpdatedBy = request.Model.UpdatedBy;
           var updateRequest = await _userRepository.UpdateAsync(updatedUser);
            var saveChangesResult = await _userRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for user with id: {request.Id}");
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
           


        }
    }
}
