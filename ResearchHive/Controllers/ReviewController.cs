using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Commands.Review;
using Application.Services.Queries.Research;
using ResearchHive.Wrapper;
using ResearchHive.DTOs;
using ResearchHive.Implementations.Services;
using Application.Services.Commands.Research;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]/*
        [ValidateAntiForgeryToken]*/
        [Produces(typeof(Result<Guid>))]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] CreateReviewRequest.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("delete")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteReview([FromBody] DeleteReview.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<ReviewDTO>>))]
        public async Task<IActionResult> GetAllReviews([FromQuery] GetAllReviews.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("reviewsbyproject")]
        [Produces(typeof(Result<IEnumerable<ReviewDTO>>))]
        public async Task<IActionResult> GetReviewsByProject([FromQuery] GetReviewsByProjectId.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("reviewsbyresearch")]
        [Produces(typeof(Result<IEnumerable<ReviewDTO>>))]
        public async Task<IActionResult> GetReviewsByResearch([FromQuery] GetReviewsByResearchId.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReview.Request request)
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
