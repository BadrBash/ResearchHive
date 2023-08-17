using Application.DTOs.UserDtos;
using Application.DTOs;
using Model.Entities;

namespace ResearchHive.Abstractions
{
    public interface IAuthenticationService
    {
        Task<LoginResponseModel> LoginAsync(string emailOrUserNameOrMatricNumber, string password);
       
        Task<DeleteUserReponseModel> DeleteAsync(string email);

        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByUserIdAsync(Guid userId);
        Task<GeneralBaseResponse> EditUserAsync(UpdateUserRequestModel model);

    }
}
