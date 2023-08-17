using Application.Abstractions.Data.Auth;
using Application.DTOs;
using Application.DTOs.UserDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using AtHackers.Unifier;
using ResearchHive.Interfaces.Repositories;
using Model.Entities;
using Model.Extensions;
using Microsoft.EntityFrameworkCore;
using ResearchHive.Constants;

namespace Persistence.Auth;

public class UserAuthenticationRepository : IUserAuthenticationRepository
{
    /*private readonly UserManager<User> _userManager;*/
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostingEnvironment;
  /*  private readonly IEmailService _emailService;*/
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserAuthenticationRepository(IConfiguration configuration,
                                            IHostEnvironment hostingEnvironment,
                                             IUserRepository userRepository)
    {
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        _userRepository = userRepository;
    }

    public async Task<UserResult> RegisterUserAsync(RegisterUserRequestModel userRegistration)
    {
        MailAddress address = new(userRegistration.Email);

        try
        {
            var user = new User(userRegistration.UserName, userRegistration.Email, AtHackerHashProvider.GenerateHash(userRegistration.Password), userRegistration.FirstName, userRegistration.LastName, userRegistration.PhoneNumber)
            {
                CreatedBy = userRegistration.CreatedBy,
                
            };

            var result = await _userRepository.AddAsync(user);
            var role = await _roleRepository.Query(ro => ro.Name == userRegistration.Role.GetDescription()).FirstOrDefaultAsync();
            if(result != null)
            {
                result.UserRoles.Add(new UserRole(result.Id, role.Id)
                {
                    Role = role,
                    User = result
                });
                await _userRepository.UpdateAsync(result);
                var saveResult = await _userRepository.SaveChangesAsync();
                if(saveResult == 1)
                {
                    return new UserResult
                    {
                        Id = user.Id,
                        Succeeded = true
                    };
                }
                return new UserResult
                {
                    Id = user.Id,
                    Succeeded = false
                };

            }

            return new UserResult
            {
                Succeeded = false,
            };
            
        }
        catch (Exception)
        {

            throw;
        }
    }
}
