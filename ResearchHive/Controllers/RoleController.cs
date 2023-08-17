using Application.Services;
using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

       
    }
}
