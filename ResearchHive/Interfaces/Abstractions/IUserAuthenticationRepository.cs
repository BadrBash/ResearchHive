using Application.DTOs;
using Application.DTOs.UserDtos;
using Model.Entities;

namespace Application.Abstractions.Data.Auth
{
    public interface IUserAuthenticationRepository
    {
        Task<UserResult> RegisterUserAsync(RegisterUserRequestModel userForRegistration);
       
    }
}
