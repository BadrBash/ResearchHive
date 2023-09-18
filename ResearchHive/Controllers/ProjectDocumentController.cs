using ResearchHive.Wrapper;
using Model.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResearchHive.Implementations.Services;
using Application.Services.Commands.ProjectDocument;
using Application.Services.Queries.ProjectDocument;
using System.Security.Claims;
using static Application.Services.Queries.ProjectDocument.GetAllProjectDocuments;
using Implementations.Services.CQRS.Queries.ProjectDocument;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectDocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        /*[ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([FromForm] CreateProjectDocumentRequest.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateProjectDocument([FromBody] UpdateProjectDocument.Request request)
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
        public async Task<IActionResult> DeleteProjectDocument([FromBody] DeleteProjectDocument.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<ProjectDocumentDTO>>))]
        public async Task<IActionResult> GetAllProjectDocuments([FromQuery] GetAllProjectDocuments.Request request)
        {

            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("student/ProjectDocuments")]
        [Produces(typeof(Result<IEnumerable<ProjectDocumentDTO>>))]
        public async Task<IActionResult> GetAllStudentProjectDocuments([FromQuery] GetProjectDocumentsByStudentId.Request request)
        {
            request.StudentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("project/{projectId}/ProjectDocuments")]
        [Produces(typeof(Result<IEnumerable<ProjectDocumentDTO>>))]
        public async Task<IActionResult> GetAllProjectDocumentsByProject([FromQuery] GetProjectDocumentsByProjectId.Request request, [FromRoute] Guid projectId)
        {
            request.ProjectId = projectId;
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("ProjectDocument/{id}")]
        [Produces(typeof(Result<ProjectDocumentDTO>))]
        public async Task<IActionResult> GetProjectDocument([FromRoute] Guid id)
        {
            var request = new GetProjectDocumentById.Request()
            {
                Id = id
            };
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

    }
}
