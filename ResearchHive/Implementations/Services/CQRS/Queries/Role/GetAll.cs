using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Role;

public class GetAllRoles
{
   public class RoleDto
    {
        public string RoleName { get; set; }
        public Guid Id { get; set;}
    }
    public record Request() : IQuery<IEnumerable<RoleDto>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public Handler(IRoleRepository roleRepository) =>
        (_roleRepository) = (roleRepository);

        public async Task<Result<IEnumerable<RoleDto>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.Query()
                .ToListAsync();
            var rolesDTO = roles.Select(role => new RoleDto
            {
                Id = role.Id,
                RoleName = role.Name,   
            });
            if(roles.Count() <= 0) return await  Result<IEnumerable<RoleDto>>.FailAsync("roles' retrieval returned empty data");
            return await  Result<IEnumerable<RoleDto>>.SuccessAsync(rolesDTO, "roles' retrieval successful");
           
        }
    }
}
