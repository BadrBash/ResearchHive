using Application.Abstractions.Data.Auth;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Application.DTOs.UserDtos;
using ResearchHive.Wrapper;
using Model.Entities;
using ResearchHive.Services;
using ResearchHive.Abstractions;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAuthenticationService _authService;
        public AuthorizationController(IMediator mediator, IUserAuthenticationRepository userAuthenticationRepository, IWebHostEnvironment hostingEnvironment, IAuthenticationService authService)
        {
            _mediator = mediator;
            _userAuthenticationRepository = userAuthenticationRepository;
            _hostingEnvironment = hostingEnvironment;
            _authService = authService;
        }



        [HttpPut]
        [Route("addtorole")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> Update([FromBody] AddUserToRoleRequest.Request request)
        {
            
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        /*[Authorize(Roles = "Administrator")]*/
        [Route("subadmin")]
        /*[ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> CreateSubAdministrator([FromBody] CreateSubAdministratorRequest.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("admin")]
        /* [ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAdministrator([FromBody] CreateAdministrator.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("lecturer")]
        /*        [ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> CreateLecturer([FromBody] CreateLecturerRequest.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpPost]
        [Route("student")]
        /*[ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest.Request request)
        {
            bool isValid = ValidatePassword(request.Password);
            if (!isValid)
            {
                return BadRequest("Password is invalid. Password must be at least 8 characters long, contain " +
                    " at least one capital letter, and a special character.");
            }
            if (User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        [Produces(typeof(AuthResponse))]
        public async Task<IActionResult> Login([FromQuery] string emailOrUserNameOrMatricNumber, [FromQuery] string password)
        {
            var result = await  _authService.LoginAsync(emailOrUserNameOrMatricNumber, password);
            return result.Status ? Ok(result) : BadRequest(result);
        }
        
       /*
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendForgotPasswordMail([FromQuery] string userNameOrEmail)
        {
            try
            {
                var user = await _userManager.Users.Where(user => user.NormalizedUserName == userNameOrEmail.ToUpper() || user.NormalizedEmail == userNameOrEmail.ToUpper()).SingleOrDefaultAsync();

                if (user is null)
                {
                    return Ok($"No user found with email or username {userNameOrEmail}");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ResetPassword), "Authorization", new { token, email = user.Email }, protocol: Request.Scheme);
                var mailResponse = await _userAuthenticationRepository.SendForgotPasswordMailAsync(user, confirmationLink);

                if (!mailResponse.Status)
                {
                    return Content(mailResponse.Message);
                }

                if (mailResponse.Status)
                {

                    return Ok(
                        new
                        {
                            status = true,
                            message = mailResponse.Message,

                        });
                }

                return BadRequest(new
                {
                    status = false,
                    message = mailResponse.Message
                });
            }
            catch
            {
                return Ok(new
                {
                    status = "error",
                    message = "Something happened. Please try again later."
                });
            }
        }
*/



        /*
        public IActionResult ResetPassword([FromQuery] string email, [FromQuery] string token)
        {
            ViewData["email"] = email;
            ViewData["token"] = token;
            return View();
        }*/
        private string createEmailBody(User user, string subject, string message, string? url = null)
        {

            string body = string.Empty;

            string basePath = _hostingEnvironment.ContentRootPath;

            using (StreamReader reader = new StreamReader(basePath + $"wwwroot\\feedbackEmail.html"))
            {

                body = reader.ReadToEnd();
            }

            body = body.Replace("{Othername}", user.FirstName); //replacing the required things  

            body = body.Replace("{Subject}", subject);

            body = body.Replace("{Message}", message);

            return body;

        }


        private static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[@#$%^&+=])(?=.{8,})");
            return regex.IsMatch(password);
        }
    }
}
