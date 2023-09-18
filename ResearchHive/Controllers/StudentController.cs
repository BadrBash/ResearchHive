using Application.Services.Commands.student;
using Application.Services.Commands.Student;
using Application.Services.Queries.Student;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ResearchHive.DTOs;
using ResearchHive.Implementations.Services;
using ResearchHive.Wrapper;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        


        [HttpPut]
        [Route("update")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudent.Request request, [FromRoute] Guid id)
        {
            request.Id = id;    
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
        public async Task<IActionResult> DeleteStudent([FromBody] DeleteStudent.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        
        [HttpPut]
        [Route("assignsupervisor_tostudent")]
        [Produces(typeof(Result<Guid>))]
        public async Task<IActionResult> AssignSupervisorToStudent([FromQuery] AssignSupervisorToStudent.Request request)
        {
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [Produces(typeof(Result<IEnumerable<StudentDTO>>))]
        public async Task<IActionResult> GetAllStudents([FromQuery] GetAllStudents.Request request)
        {

            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
        
        [HttpGet]
        [Route("students/{departmentId}")]
        [Produces(typeof(Result<IEnumerable<StudentDTO>>))]
        public async Task<IActionResult> GetStudentsByDepartment([FromQuery] GetStudentsByDepartment.Request request, [FromRoute] Guid departmentId)
        {
            request.DepartmentId = departmentId;
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("students/{id}")]
        [Produces(typeof(Result<StudentDTO>))]
        public async Task<IActionResult> GetStudent([FromQuery] GetById.Request request, [FromRoute] Guid id)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
