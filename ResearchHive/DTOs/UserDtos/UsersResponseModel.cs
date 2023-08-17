using Application.DTOs;
using ResearchHive.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDtos
{
    public class UsersResponseModel : BaseResponse<PaginatedList<UserDto>>
    {
        public PaginatedList<UserDto> Data { get; set; } = new PaginatedList<UserDto>();
    }
}
