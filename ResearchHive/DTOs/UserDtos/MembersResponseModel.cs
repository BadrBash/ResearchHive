using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDtos
{
    public class MembersResponseModel : BaseResponse<IEnumerable<UserDto>>
    {
        public IEnumerable<UserDto> Members { get; set; }
    }
}
