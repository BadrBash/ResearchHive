using Application.Services;
using ResearchHive.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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



        

    }
}
