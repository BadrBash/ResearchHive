
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.User;

public class DeleteUser
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository) =>
        (_userRepository) = (userRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(usr => usr.Id == request.Id);
            if (user is null) return await Result<string>.FailAsync($"user with id: {request.Id} not found");
            await _userRepository.DeleteAsync(user);
            var saveChangesResult = await _userRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for user with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);



        }
    }
}
