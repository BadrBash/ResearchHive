using Application.Abstractions.Data.Auth;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Enums;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CreateAdministrator
    {
        public record Request(string FirstName, string LastName,
            string EmailAddress, string PhoneNumber, string UserName) : ICommand<Guid>
        {

            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public class Handler : ICommandHandler<Request, Guid>
        {
            private readonly IAdministratorRepository _administratorRepository;
            private readonly IUserRepository _userRepository;
            private readonly IConfiguration _config;
            private readonly IUserAuthenticationRepository _userAuthenticationRepository;

            public Handler(IAdministratorRepository administratorRepository,
                IUserRepository userRepository, IUserAuthenticationRepository userAuthenticationRepository, IConfiguration configuration)
            {
                _config = configuration.GetSection("SkC");
                _administratorRepository = administratorRepository;
                _userRepository = userRepository;
                _userAuthenticationRepository = userAuthenticationRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var userExists = await _userRepository.ExistsAsync(us => us.Email == request.EmailAddress);
                var adminUserCount = (await _administratorRepository.Query().ToListAsync(cancellationToken)).Count;
                if (userExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }

                if(adminUserCount == 2)
                {
                    return await Result<Guid>.ReturnErrorAsync(ResponseMessage.ContactAdminForAccess);
                }
                var userResult = await _userAuthenticationRepository.RegisterUserAsync(
                    new RegisterUserRequestModel
                    {
                        UserName = request.UserName,
                        Email = request.EmailAddress,
                        PhoneNumber = request.PhoneNumber,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Password = _config["admin"],
                        Role = Roles.Administrator,
                    });
                if(userResult.Succeeded)
                {
                    var administrator = new Administrator(userResult.Id)
                    {
                         CreatedBy = request.CreatedBy,
                    };

                    var adminResult = await _administratorRepository.AddAsync(administrator);
                    var saveChangesResult = await _administratorRepository.SaveChangesAsync(cancellationToken);
                    if(saveChangesResult == 1)
                    {
                        return await Result<Guid>.SuccessAsync(ResponseMessage.CreationSuccesful + $"{_config["admin"]}");
                    }
                    
                    return  await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
                }

                return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);



            }
        }
    }
}
