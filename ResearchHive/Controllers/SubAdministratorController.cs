using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResearchHive.Wrapper;
using System.Security.Claims;

namespace ResearchHive.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubAdministratorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubAdministratorController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
