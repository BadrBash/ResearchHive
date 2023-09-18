
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.User;

public class GetById
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> UserRoles { get; set; }

    }
    public record Request(Guid id) : IQuery<UserDto>;

    public record Handler : IQueryHandler<Request, UserDto>
    {
        private readonly IUserRepository _UserRepository;

        public Handler(IUserRepository UserRepository) =>
        (_UserRepository) = (UserRepository);

        public async Task<Result<UserDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var User = await _UserRepository.GetUserAsync(usr => usr.Id == request.id);
            if (User is null) return await Result<UserDto>.FailAsync($"User with id: {request.id} not found");
            var UserReturned = new UserDto
            {
                Email = User.Email,
                UserId = User.Id,
                UserName = User.UserName,
                FirstName = User.FirstName,
                LastName = User.LastName,
                PhoneNumber = User.PhoneNumber,
                UserRoles = User.UserRoles.Select(us => us.Role.Name).ToList()
            };
            return await Result<UserDto>.SuccessAsync(UserReturned, $"User with id: {request.id} successfully retrieved");


        }
    }
}
