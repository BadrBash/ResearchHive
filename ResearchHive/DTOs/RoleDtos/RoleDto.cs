using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.DTOs.RoleDtos
{
    public class RoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
    }
}
