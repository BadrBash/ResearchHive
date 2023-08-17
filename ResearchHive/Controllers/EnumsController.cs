using Microsoft.AspNetCore.Mvc;
using Model.Enums;

namespace TheLogoPhilia.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EnumsController : ControllerBase
    {
        [HttpGet("levels")]
        public IActionResult GetLevels()
        {
            var levels = Enum.GetValues(typeof(Level)).Cast<int>().ToList();
            List<string> levelsList = new();
            foreach (var item in levels)
            {
                levelsList.Add(Enum.GetName(typeof(Level),item));
            }
            return Ok(levelsList);

        }
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = Enum.GetValues(typeof(Roles)).Cast<int>().ToList();
            List<string> rolesList = new();
            foreach (var item in roles)
            {
                rolesList.Add(Enum.GetName(typeof(Roles),item));
            }
            return Ok(rolesList);

        }
        [HttpGet("stages")]
        public IActionResult GetStages()
        {
            var compTypes = Enum.GetValues(typeof(ProjectStage)).Cast<int>().ToList();
            List<string> CompetitionTypes = new();
            foreach (var item in compTypes)
            {
                CompetitionTypes.Add(Enum.GetName(typeof(ProjectStage),item));
            }
            return Ok(CompetitionTypes);

        }


    }
}