
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Role;

public class UpdateRole
{

    public record Request() : IQuery<string>
    {
        [JsonIgnore] public Guid UpdatedBy { get; set; }
        [JsonIgnore] public Guid Id { get; set; }
    }

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IRoleRepository _RoleRepository;
        public Handler(IRoleRepository RoleRepository) =>
        (_RoleRepository) = (RoleRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Role = await _RoleRepository.Query(pd => pd.Id == request.Id).SingleOrDefaultAsync();
            if (Role is null) return await Result<string>.FailAsync($"Role with id: {request.Id} not found");
            var updateRequest = await _RoleRepository.UpdateAsync(Role);
            var saveChangesResult = await _RoleRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);

        }
    }
}
