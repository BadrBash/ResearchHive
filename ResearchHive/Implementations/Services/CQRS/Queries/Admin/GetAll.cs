using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Admin;

public class GetAllAdmins
{
    public class AdminDto
    {
        public Guid UserId { get; set; }
        public Guid Id { get;  set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get;   set; }
        public string PhoneNumber { get;   set; }
        public ICollection<string> UserRoles { get;   set; }

    }
    public record Request() : IQuery<IEnumerable<AdminDto>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<AdminDto>>
    {
          private readonly IAdministratorRepository _adminRepository;

        public Handler(IAdministratorRepository adminRepository) =>
        (_adminRepository) = (adminRepository);

        public async Task<Result<IEnumerable<AdminDto>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var admins = await _adminRepository.Query().Include(adm => adm.User).ThenInclude(us => us.UserRoles).ThenInclude(ur => ur.Role).ToListAsync(cancellationToken);
            var adminsDto = admins.Select(admin => new AdminDto
            {
                Email = admin.User.Email,
                Id = admin.Id,
                UserId = admin.UserId,
                UserName = admin.User.UserName,
                FirstName = admin.User.FirstName,
                LastName = admin.User.LastName,
                PhoneNumber = admin.User.PhoneNumber,
                UserRoles = admin.User.UserRoles.Select(us => us.Role.Name).ToList()
            });
            if(admins.Count() <= 0) return await  Result<IEnumerable<AdminDto>>.FailAsync("Admins' retrieval returned empty data");
            return await  Result<IEnumerable<AdminDto>>.SuccessAsync(adminsDto, "Admins' retrieval successful");
           
        }
    }
}
