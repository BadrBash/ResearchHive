using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ResearchHive.Implementations.Services;
using Application.Services.Commands.Department;
using Application.Services.Queries.Department;
using static Application.Services.Queries.Department.GetAll;
using Application.Services.Commands.Admin;

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
        //[ValidateAntiForgeryToken]
        [Produces(typeof(Result<Guid>))]
        //[Authorize(Roles = "Administrator, Sub Administrator")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.CreatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartment.Request request)
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
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
        {
            var request = new DeleteDepartment.Request(id);
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<DepartmentDto>>))]
        public async Task<IActionResult> GetAllDepartments([FromQuery] GetAll.Request request)
        {

            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("dept/{id}")]
        [Produces(typeof(Result<DepartmentDto>))]
        public async Task<IActionResult> GetDepartment([FromRoute] Guid id)
        {
            var request = new GetDeptById.Request
            {
                Id = id,
            };
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        } 
        
        [HttpGet]
        [Route("dept/name")]
        [Produces(typeof(Result<DepartmentDto>))]
        public async Task<IActionResult> GetByName([FromQuery] GetDeptByName.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
