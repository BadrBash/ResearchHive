using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResearchHive.Implementations.Services;
using Application.Services.Commands.Project;
using Application.Services.Queries.Project;
using System.Security.Claims;
using static Application.Services.Queries.Project.GetAllProjects;
using ResearchHive.Implementations.Services.CQRS.Queries.Project;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> Create([FromForm] CreateProjectRequest.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("approve")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> ApproveProject([FromBody] ApproveProject.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpPut]
        [Route("complete")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> CompleteProject([FromBody] CompleteSubmission.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        } 
        
        [HttpPut]
        [Route("setinprogress")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> SetInProgress([FromBody] SetProjectInProgress.Request request)
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
        public async Task<IActionResult> DeleteProject([FromBody] DeleteProject.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetAllProjects([FromQuery] GetAllProjects.Request request)
        {

            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("studentprojects")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetAllStudentProjects([FromQuery] GetProjectsByStudentId.Request request)
        {
            request.StudentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("Project/{id}")]
        [Produces(typeof(Result<ProjectDTO>))]
        public async Task<IActionResult> GetProject([FromQuery] GetProjectById.Request request, [FromRoute] Guid id)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("approved")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetApprovedProjects([FromQuery] GetApprovedProjects.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        } 
        
        [HttpGet]
        [Route("completed")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetCompletedProjects([FromQuery] GetCompletedProjects.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        } 
        
        [HttpGet]
        [Route("incomplete")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetIncompleteProjects([FromQuery] GetIncompleteProjects.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("bystage")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetProjectsByStage([FromQuery] GetProjectsByStage.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        } 
        
        [HttpGet]
        [Route("bycurrentyear")]
        [Produces(typeof(Result<IEnumerable<ProjectDTO>>))]
        public async Task<IActionResult> GetProjectsByCurrentYear([FromQuery] GetProjectsByYear.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
    }
}
