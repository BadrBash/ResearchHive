using Application.Services;
using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Services.Commands.Lecturer;
using Application.Services.Queries.Lecturer;
using static Application.Services.Queries.Lecturer.GetAll;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LecturerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateLecturer([FromBody] UpdateLecturer.Request request)
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
        public async Task<IActionResult> DeleteLecturer([FromRoute] Guid id )
        {
            DeleteLecturer.Request request = new()
            {
                Id = id,
            };
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<LecturerDto>>))]
        public async Task<IActionResult> GetAllLecturers([FromQuery] GetAll.Request request)
        {

            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces(typeof(Result<LecturerDto>))]
        public async Task<IActionResult> GetLecturer([FromQuery] GetById.Request request, [FromRoute] Guid id)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("staffnumber")]
        [Produces(typeof(Result<LecturerDto>))]
        public async Task<IActionResult> GetByName([FromQuery] GetByStaffNumber.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("logged-in")]
        [Produces(typeof(Result<LecturerDto>))]
        public async Task<IActionResult> GetByName([FromQuery] GetByUserId.Request request)
        {
            request.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

    }
}
