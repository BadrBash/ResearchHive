using Application.DTOs.UserDtos;

namespace ResearchHive.Authentication
{
    public interface IJWTTokenHandler
    {
        string GenerateToken(LoginResponseData Model);
    }
}