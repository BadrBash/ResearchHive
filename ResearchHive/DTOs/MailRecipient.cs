using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class MailRecipient
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
