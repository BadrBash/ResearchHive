using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs.UserDtos;
using Microsoft.IdentityModel.Tokens;

namespace ResearchHive.Authentication
{
    public class JWTTokenHandler : IJWTTokenHandler
    {
        private readonly string Key;

        public JWTTokenHandler(string key)
        {
            Key = key;
        }

        public string GenerateToken(LoginResponseData responseData)
        {
           var tokenHandler = new JwtSecurityTokenHandler();

            List<Claim> Claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, responseData.UserId.ToString()),
                new Claim(ClaimTypes.Name, responseData.UserName),
                new Claim("FullName", $"{responseData.FirstName} {responseData.LastName}"),
                   new Claim(ClaimTypes.Email, responseData.Email)

            };
            foreach (var item in responseData.Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, item));
            }
           var key = Encoding.ASCII.GetBytes(Key);
           var tokenDescriptor = new SecurityTokenDescriptor
           {
                Subject = new ClaimsIdentity(Claims),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
           };
           var token = tokenHandler.CreateToken(tokenDescriptor);
           return tokenHandler.WriteToken(token);
        }
    }
}