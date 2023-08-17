using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserResult 
    {
        public Guid Id { get; set; }
        public  bool Succeeded { get; set; }
        

    }
}
