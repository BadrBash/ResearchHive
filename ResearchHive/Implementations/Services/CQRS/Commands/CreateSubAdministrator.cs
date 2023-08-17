using Application.Abstractions.Data.Auth;
using Application.Abstractions.Messaging;
using ResearchHive.Constants;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Interfaces.Repositories;
using System.Text.Json.Serialization;
using Model.Enums;

namespace Application.Services
{
    public class CreateSubAdministratorRequest
    {
        public record Request(string FirstName, string LastName,
            string EmailAddress, string PhoneNumber ) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly IAdministratorRepository _administratorRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserAuthenticationRepository _userAuthenticationRepository;
            private readonly IConfiguration _config;
            public Handler(IAdministratorRepository administratorRepository, IUserRepository userRepository, IUserAuthenticationRepository userAuthenticationRepository, IConfiguration configuration)
            {
                _config = configuration.GetSection("SkC");
                _administratorRepository = administratorRepository;
                _userRepository = userRepository;
                _userAuthenticationRepository = userAuthenticationRepository;
            }

            public async  Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var userExists = await _userRepository.ExistsAsync(us => us.Email == request.EmailAddress);
                if (userExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }
                var userResult = await _userAuthenticationRepository.RegisterUserAsync(
                    new RegisterUserRequestModel
                    {
                        Email = request.EmailAddress,
                        PhoneNumber = request.PhoneNumber,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Password = $"{_config["admin"]}",
                        Role = Roles.SubAdministrator,
                    });
                if (userResult.Succeeded)
                {
                    var administrator = new Administrator(userResult.Id)
                    {
                        CreatedBy = request.CreatedBy,
                    };

                    var adminResult = await _administratorRepository.AddAsync(administrator);
                    var saveChangesResult = await _administratorRepository.SaveChangesAsync(cancellationToken);
                    if (saveChangesResult == 1)
                    {
                        return await Result<Guid>.SuccessAsync(ResponseMessage.CreationSuccesful + $"{_config["admin"]}");
                    }

                    return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
                }

                return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
            }
        }
    }
}
