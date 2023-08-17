using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class ApiTokenModel : ApiResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
