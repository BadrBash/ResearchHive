using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.User;

public class GetAllUsers
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get;   set; }
        public string PhoneNumber { get;   set; }
        public ICollection<string> UserRoles { get;   set; }

    }
    public record Request() : IQuery<IEnumerable<UserDto>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<UserDto>>
    {
          private readonly IUserRepository _UserRepository;

        public Handler(IUserRepository UserRepository) =>
        (_UserRepository) = (UserRepository);

        public async Task<Result<IEnumerable<UserDto>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Users = await _UserRepository.Query().Include(adm => adm.UserRoles).
                Include(us => us.Administrator).
                Include(ur => ur.Student).
                Include(us => us.Administrator).
                ToListAsync();
            var UsersDto = Users.Select(User => new UserDto
            {
                Email = User.Email,
                UserId = User.Id,
                UserName = User.UserName,
                FirstName = User.FirstName,
                LastName = User.LastName,
                PhoneNumber = User.PhoneNumber,
                UserRoles = User.UserRoles.Select(us => us.Role.Name).ToList()
            });
            if(Users.Count() <= 0) return await  Result<IEnumerable<UserDto>>.FailAsync("Users' retrieval returned empty data");
            return await  Result<IEnumerable<UserDto>>.SuccessAsync(UsersDto, "Users' retrieval successful");
           
        }
    }
}
