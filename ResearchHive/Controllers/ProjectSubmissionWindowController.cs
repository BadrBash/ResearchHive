using ResearchHive.Wrapper;
using Model.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Commands.ProjectSubmissionWindow;
using Application.Services.Queries.ProjectSubmissionWindow;
using System.Security.Claims;
using static Application.Services.Queries.ProjectSubmissionWindow.GetAllProjectSubmissionWindows;
using ResearchHive.Implementations.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectSubmissionWindowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectSubmissionWindowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        [Produces(typeof(Result<Guid>))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromForm] CreateProjectSubmissionWindowRequest.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateProjectSubmissionWindow([FromBody] UpdateProjectSubmissionWindow.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.Model.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpPut]
        [Route("closeprojectsubmissionwindow")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> CloseProjectSubmissionWindow([FromBody] CloseProjectSubmission.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("delete")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteProjectSubmissionWindow([FromBody] DeleteProjectSubmissionWindow.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<ProjectSubmissionWindowDTO>>))]
        public async Task<IActionResult> GetAllProjectSubmissionWindows([FromQuery] GetAllProjectSubmissionWindows.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("getallavailable")]
        [Produces(typeof(Result<IEnumerable<ProjectSubmissionWindowDTO>>))]
        public async Task<IActionResult> GetAvailableProjectSubmissionWindows([FromQuery] GetAvailableProjectSubmissionWindows.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

       
    }
}
