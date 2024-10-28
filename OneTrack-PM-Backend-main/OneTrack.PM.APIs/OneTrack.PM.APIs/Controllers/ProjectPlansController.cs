using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.Core.Repositories;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OneTrack.PM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectPlanController : ControllerBase
    {
        private readonly IProjectPlanRepository _projectPlanRepository;

        public ProjectPlanController(IProjectPlanRepository projectPlanRepository)
        {
            _projectPlanRepository = projectPlanRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectPlan([FromBody] ProjectPlan projectPlan)
        {
            if (projectPlan == null) return BadRequest("Project plan data is missing.");

            var createdPlan = await _projectPlanRepository.CreateProjectPlanAsync(projectPlan);
            return CreatedAtAction(nameof(GetProjectPlanById), new { id = createdPlan.Id }, createdPlan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectPlan(int id, [FromBody] ProjectPlan updatedProjectPlan)
        {
            if (updatedProjectPlan == null || id != updatedProjectPlan.Id) return BadRequest("Invalid project plan data.");

            var projectPlan = await _projectPlanRepository.GetProjectPlanByIdAsync(id);
            if (projectPlan == null) return NotFound();

            projectPlan.Name = updatedProjectPlan.Name;
            projectPlan.Description = updatedProjectPlan.Description;
            projectPlan.StartDate = updatedProjectPlan.StartDate;
            projectPlan.EndDate = updatedProjectPlan.EndDate;
            projectPlan.Status = updatedProjectPlan.Status;

            await _projectPlanRepository.UpdateProjectPlanAsync(projectPlan);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectPlan(int id)
        {
            var projectPlan = await _projectPlanRepository.GetProjectPlanByIdAsync(id);
            if (projectPlan == null) return NotFound();

            await _projectPlanRepository.DeleteProjectPlanAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProjectPlans([FromQuery] string search, int pageNumber = 1, int pageSize = 10)
        {
            var pagedProjectPlans = await _projectPlanRepository.GetPagedProjectPlansAsync(search, pageNumber, pageSize);
            return Ok(pagedProjectPlans);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetProjectPlans([FromQuery] string search)
        {
            var projectPlans = await _projectPlanRepository.GetProjectPlansAsync(search);
            return Ok(projectPlans);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeProjectPlanStatus(int id, [FromBody] string status)
        {
            var projectPlan = await _projectPlanRepository.GetProjectPlanByIdAsync(id);
            if (projectPlan == null) return NotFound();

            projectPlan.Status = status;
            await _projectPlanRepository.UpdateProjectPlanAsync(projectPlan);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectPlanById(int id)
        {
            var projectPlan = await _projectPlanRepository.GetProjectPlanByIdAsync(id);
            if (projectPlan == null) return NotFound();

            return Ok(projectPlan);
        }
    }
}
