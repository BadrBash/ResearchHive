using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Services.Commands.Role;
using Application.Services.Queries.Role;
using RoleDto = Application.Services.Queries.Role.GetAllRoles.RoleDto;
using ResearchHive.Implementations.Services;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
      /*  [ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest.Request request)
        {
            if(User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
           
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("delete")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRole.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getroles")]
        [Produces(typeof(Result<IEnumerable<RoleDto>>))]
        public async Task<IActionResult> GetAllRoles([FromQuery] GetAllRoles.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        

        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRole.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

    }
}
