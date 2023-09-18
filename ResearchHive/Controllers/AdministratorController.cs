using Application.Services;
using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Services.Commands.Admin;
using Application.Services.Queries.Admin;
using static Application.Services.Queries.Admin.GetAllAdmins;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdministratorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdmin.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.Model.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
         
        [HttpPut]
        [Route("delete/{id}")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAdmin([FromRoute] Guid id)
        {
            var request = new DeleteAdmin.Request(id);
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        } 
        
        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<AdminDto>>))]
        public async Task<IActionResult> GetAllAdmins([FromQuery] GetAllAdmins.Request request)
        {
           
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("admins/{id}")]
        [Produces(typeof(Result<AdminDto>))]
        public async Task<IActionResult> GetAdmin([FromQuery] GetById.Request request, [FromRoute] Guid id)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
