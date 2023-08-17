using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RoleDtos
{
    public class RoleResponseModel : BaseResponse<RoleDto>
    {
        public RoleDto Data { get; set; }
    }
}
