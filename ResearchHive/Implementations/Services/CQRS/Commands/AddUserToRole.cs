using ResearchHive.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using ResearchHive.Interfaces.Repositories;
using Application.Abstractions.Data.Auth;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using ResearchHive.Constants;

namespace ResearchHive.Services
{
    public class AddUserToRoleRequest
    {
        public record Request(Guid UserId) : ICommand<string>
        {

        }

        public record Handler : ICommandHandler<Request, string>
        {
            private readonly IUserRepository _userRepository;
            private readonly IRoleRepository _roleRepository;   
            private readonly IUserAuthenticationRepository _userAuthenticationRepository;

            public Handler(IUserRepository userRepository, IUserAuthenticationRepository userAuthenticationRepository, IRoleRepository roleRepository)
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _userAuthenticationRepository = userAuthenticationRepository;
            }

            public  async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
            {
                var user =  await _userRepository.GetAsync(request.UserId);
                var result = new Result<string>();
                var userRoles =  user.UserRoles.Select(x => new
                {
                    id = x.RoleId,
                    name = x.Role.Name
                }).ToList();
                #region LecturerToSupervisor
                if (userRoles.Select(x => x.name).Contains("Lecturer"))
                {
                    var role = await _roleRepository.Query(exp => exp.Name == "Supervisor").FirstOrDefaultAsync(cancellationToken);
                    user.UserRoles.Add(new UserRole(user.Id, role.Id)
                    {
                        Role = role,
                        User = user

                    });
                    await _userRepository.UpdateAsync(user);
                    var saveResult = await _userRepository.SaveChangesAsync(cancellationToken);
                    if (saveResult == 1)
                    {
                        result = await Result<string>.SuccessAsync(ResponseMessage.UpdateSuccessful);
                    }

                    result = await Result<string>.FailAsync(ResponseMessage.OperationFailed);
                }
                #endregion LecturerToSupervisor
                #region SubAdminToAdmin
                else if (userRoles.Select(x => x.name).Contains("Sub Administrator"))
                {
                    var role = await _roleRepository.Query(exp => exp.Name == "Administrator").FirstOrDefaultAsync(cancellationToken);
                    user.UserRoles.Add(new UserRole(user.Id, role.Id)
                    {
                        Role = role,
                        User = user

                    });
                    await _userRepository.UpdateAsync(user);
                    var saveResult = await _userRepository.SaveChangesAsync(cancellationToken);
                    if (saveResult == 1)
                    {
                        result = await Result<string>.SuccessAsync(ResponseMessage.UpdateSuccessful);
                    }
                    result =  await Result<string>.FailAsync(ResponseMessage.OperationFailed);
                }
                #endregion SubAdminToAdmin
                return result;
            }
        }
    }
}
