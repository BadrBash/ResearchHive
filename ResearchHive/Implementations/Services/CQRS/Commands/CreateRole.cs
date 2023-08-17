
using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using Model.Entities;
using Model.Enums;
using Model.Extensions;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CreateRoleRequest
    {
        public class Request : ICommand<Guid>
        {
            public Roles Name { get; set; }
            [JsonIgnore]
            public Guid? CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly IRoleRepository _roleRepository;

            public Handler(IRoleRepository roleRepository)
            {
                _roleRepository = roleRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var roleExists = await _roleRepository.ExistsAsync(exp => exp.Name == request.Name.GetDescription());
                if(roleExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }

                var role = new Role(request.Name.GetDescription())
                {
                    CreatedBy = request.CreatedBy
                };
                 var roleResult = await _roleRepository.AddAsync(role);
                var saveResult =  await _roleRepository.SaveChangesAsync(cancellationToken);
                if(saveResult == 1)
                {
                    return new Result<Guid>
                    {
                          Data = role.Id,
                          Messages = new List<string> { ResponseMessage.CreateSuccessful },
                          Succeeded = true
                    } ;

                }
                return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
            }
        }
    }
}
