using Application.Abstractions.Data.Auth;
using Application.DTOs;
using Application.DTOs.UserDtos;
using AtHackers.Unifier;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using ResearchHive.Abstractions;
using ResearchHive.Authentication;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;

namespace Application.Abstractions
{
    public class AuthService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJWTTokenHandler _jwtTokenHandler;
        public AuthService(IConfiguration configuration,
            IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUserAuthenticationRepository userAuthenticationRepository, IJWTTokenHandler jwtTokenHandler )
        {
          
            _configuration = configuration;
            _userRepository = userRepository;
            _userAuthenticationRepository = userAuthenticationRepository;
            _jwtTokenHandler = jwtTokenHandler;
        }
       

        public async Task<DeleteUserReponseModel> DeleteAsync(string email)
        {
           var user = await _userRepository.Query(exp => exp.Email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                var saveResponse = await _userRepository.SaveChangesAsync();
                if(saveResponse == 1)
                {
                    return new DeleteUserReponseModel
                    {
                        IsDeleted = true,
                        Message = ResponseMessage.DeleteSuccessful,
                        Status = true
                    };
                }
                return new DeleteUserReponseModel
                {
                    IsDeleted = false,
                    Message = ResponseMessage.OperationFailed,
                    Status = true
                };
            }
            return new DeleteUserReponseModel
            {
                IsDeleted = false,
                Message = ResponseMessage.RecordNotFound,
                Status = true
            };
        }

        public async Task<GeneralBaseResponse> EditUserAsync(UpdateUserRequestModel model)
        {
            var user = await _userRepository.GetAsync(model.Id);

            if (user != null)
            {
                var userUpdate = user.Update(model.FirstName, model.LastName, model.Email, model.PhoneNumber);
                var updated = await _userRepository.UpdateAsync(userUpdate);
                var saveResult = await _userRepository.SaveChangesAsync();
                if (saveResult == 1  && updated.FirstName == model.FirstName)
                {
                    return new GeneralBaseResponse(true, 200, ResponseMessage.UpdateSuccessful);
                }
                else
                {
                    return new GeneralBaseResponse(false, 500, ResponseMessage.OperationFailed);
                }
            }
            return new GeneralBaseResponse(false, 404, ResponseMessage.RecordNotFound);
        }

      

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserAsync(exp => exp.Email == email);
        }

        public async Task<UserDto> GetUserByUserIdAsync(Guid userId)
        {
            var getUser = await _userRepository.GetAsync(userId);
            var userDto = new UserDto
            {
                Id = getUser.Id,
                UserName = getUser.UserName,
                Email = getUser.Email,
                LastName = getUser.LastName,
                FirstName = getUser.FirstName,
                PhoneNumber = getUser.PhoneNumber,
            };
            return userDto;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userRepository.GetUserAsync(exp => exp.UserName == userName);
        }

        public async Task<LoginResponseModel> LoginAsync(string emailOrUserNameOrMatricNumber, string password)
        {
           
                var user = await _userRepository.GetUser(L => L.Email == emailOrUserNameOrMatricNumber || L.UserName == emailOrUserNameOrMatricNumber || L.Student.MatricNumber == emailOrUserNameOrMatricNumber);
                if (user == null || (AtHackerHashProvider.ValidatePassword(password, user.Password)) != true) return new LoginResponseModel
                {
                    Message = ResponseMessage.SignInValidationFailed,
                    Status = false
                };

                var responseModel =  new LoginResponseModel
                {
                    Message = ResponseMessage.SignInSuccessful,
                    Status = true,
                    Data = new LoginResponseData
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles  =  (await _userRepository.ManageUserRoles(user.Id)).Select(us => us.RoleName).ToList(),
                    },
                };
                responseModel.Token = _jwtTokenHandler.GenerateToken(responseModel.Data);
                return responseModel;
        }
    

      
    }
}
