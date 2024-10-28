using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using System.Collections.Generic;

namespace OneTrack.PM.Core.Repositories
{
    public interface IProjectPlanRepository
    {
        Task<ProjectPlan> CreateProjectPlanAsync(ProjectPlan projectPlan);
        Task<ProjectPlan> GetProjectPlanByIdAsync(int id);
        Task UpdateProjectPlanAsync(ProjectPlan projectPlan);
        Task DeleteProjectPlanAsync(int id);
        Task<IEnumerable<ProjectPlan>> GetPagedProjectPlansAsync(string search, int pageNumber, int pageSize);
        Task<IEnumerable<ProjectPlan>> GetProjectPlansAsync(string search);
    }
}
