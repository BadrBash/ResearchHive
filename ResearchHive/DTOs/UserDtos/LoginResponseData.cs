

namespace Application.DTOs.UserDtos
{
    public class LoginResponseData
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }

        public string LastName { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();

    }
}
