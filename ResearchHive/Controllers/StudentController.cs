using Application.Services;
using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        

        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[@#$%^&+=])(?=.{8,})");
            return regex.IsMatch(password);
        }
    }
}
