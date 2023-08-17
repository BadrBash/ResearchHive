using Application.DTOs;
using Application.DTOs.RoleDtos;
using ResearchHive.Wrapper;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserDtos
{
    public class UserDto
    {

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? DistributorId { get; set; }
        public List<RoleDto> Roles {get; set;}
        public Roles Role {get; set;}
        public string? DistributorName { get; set; }
        public string? Position { get; set; }
        public bool? IsActive { get; set; }
        public bool? ReadOnly { get; set; } = false;
        public bool? IsAdmin { get; set; } = false;
        public string Distributor => DistributorName == null ? $"-" : $"{DistributorName}";

    }
}