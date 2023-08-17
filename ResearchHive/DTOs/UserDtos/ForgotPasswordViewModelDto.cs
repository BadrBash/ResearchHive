using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDtos
{
    public class ForgotPasswordViewModelDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class PasswordMailSentResponse : GeneralBaseResponse
    {
        public PasswordMailSentResponse(bool status, int code, string message, string field = "", int count = 0, object response = null) : base(status, code, message, field, count, response)
        {
        }
    }
}
