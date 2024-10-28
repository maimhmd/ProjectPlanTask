using OneTrack.PM.Core.Repositories;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneTrack.PM.Services
{
    public class ProjectPlanService : IProjectPlanService
    {
        private readonly IProjectPlanRepository _projectPlanRepository;

        public ProjectPlanService(IProjectPlanRepository projectPlanRepository)
        {
            _projectPlanRepository = projectPlanRepository;
        }

        public async Task<ProjectPlan> CreateProjectPlanAsync(ProjectPlan projectPlan)
        {
            return await _projectPlanRepository.CreateProjectPlanAsync(projectPlan);
        }

        public async Task<ProjectPlan> GetProjectPlanByIdAsync(int id)
        {
            return await _projectPlanRepository.GetProjectPlanByIdAsync(id);
        }

        public async Task UpdateProjectPlanAsync(ProjectPlan projectPlan)
        {
            await _projectPlanRepository.UpdateProjectPlanAsync(projectPlan);
        }

        public async Task DeleteProjectPlanAsync(int id)
        {
            await _projectPlanRepository.DeleteProjectPlanAsync(id);
        }

        public async Task<IEnumerable<ProjectPlan>> GetPagedProjectPlansAsync(string search, int pageNumber, int pageSize)
        {
            return await _projectPlanRepository.GetPagedProjectPlansAsync(search, pageNumber, pageSize);
        }

        public async Task<IEnumerable<ProjectPlan>> GetProjectPlansAsync(string search)
        {
            return await _projectPlanRepository.GetProjectPlansAsync(search);
        }

        public async Task ChangeProjectPlanStatusAsync(int id, string status)
        {
            var projectPlan = await _projectPlanRepository.GetProjectPlanByIdAsync(id);
            if (projectPlan != null)
            {
                projectPlan.Status = status;
                await _projectPlanRepository.UpdateProjectPlanAsync(projectPlan);
            }
        }
    }
}
