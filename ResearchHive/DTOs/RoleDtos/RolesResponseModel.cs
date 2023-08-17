using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RoleDtos
{
    public class RolesResponseModel : BaseResponse<IEnumerable<RoleDto>>
    {
        public IEnumerable<RoleDto> Data { get; set; } = new List<RoleDto>();
    }
}
