using Application.Services;
using ResearchHive.Wrapper;
using Model.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
