using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Commands.Research;
using Application.Services.Queries.Research;
using System.Security.Claims;
using static Application.Services.Queries.Research.GetAllResearches;
using ResearchHive.Implementations.Services;
using Application.Services.Queries.Project;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        [Produces(typeof(Result<Guid>))]
        [Authorize(Roles = "Administrator, SubAdministrator")]
        public async Task<IActionResult> Create([FromForm] CreateResearchRequest.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpPut]
        [Route("approve")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> ApproveResearch([FromBody] ApproveResearch.Request request)
        {
            if (User.Identity.IsAuthenticated)
            {
                request.UpdatedBy = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateResearch([FromBody] UpdateResearch.Request request)
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
        public async Task<IActionResult> DeleteResearch([FromBody] DeleteResearch.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<ResearchDTO>>))]
        public async Task<IActionResult> GetAllResearches([FromQuery] GetAllResearches.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getallapproved")]
        [Produces(typeof(Result<IEnumerable<ResearchDTO>>))]
        public async Task<IActionResult> GetApprovedResearches([FromQuery] GetApprovedResearches.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("research/{id}")]
        [Produces(typeof(Result<ResearchDTO>))]
        public async Task<IActionResult> GetResearch([FromQuery] GetResearchById.Request request, [FromRoute] Guid id)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("research/{authorId}")]
        [Produces(typeof(Result<ResearchDTO>))]
        public async Task<IActionResult> GetResearchesByAuthorId([FromQuery] GetResearchesByAuthorId.Request request, [FromRoute] Guid authorId)
        {
            request.AuthorId = authorId;

            if (User.Identity.IsAuthenticated)
            {
                request.AuthorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

    }
}
