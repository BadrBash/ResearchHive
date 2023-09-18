using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Role;

public class GetById
{
    public class RoleDto
    {
        public string RoleName { get; set; }
        public Guid Id { get; set; }
    }
    public record Request(Guid RoleId) : IQuery<RoleDto>;

    public record Handler : IQueryHandler<Request, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;

        public Handler(IRoleRepository roleRepository) =>
        (_roleRepository) = (roleRepository);

        public async Task<Result<RoleDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.Query(role => role.Id == request.RoleId && !role.IsDeleted).SingleOrDefaultAsync();
            var roleDTO = new RoleDto
            {
                Id = role.Id,
                RoleName = role.Name,
            };
            if (role == null) return await Result<RoleDto>.FailAsync("role  retrieval returned empty data");
            return await Result<RoleDto>.SuccessAsync(roleDTO, "role retrieval successful");

        }
    }
}
