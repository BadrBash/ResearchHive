using Application.Services;
using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        [Produces(typeof(Result<Guid>))]
        [Authorize(Roles = "Administrator, Sub Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
