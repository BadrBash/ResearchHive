using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDtos
{
    public class LoginResponseModel : BaseResponse<LoginResponseData>
    {
        public LoginResponseData Data { get; set; }
        public string Token { get; set; }
    }
}
