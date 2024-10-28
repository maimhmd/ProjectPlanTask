using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.DTOs.Shared;
using OneTrack.PM.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTrack.PM.Core.Services
{
    public interface IProjectPlanService
    {
        Task<ProjectPlan> CreateProjectPlanAsync(ProjectPlan projectPlan);
        Task<ProjectPlan> GetProjectPlanByIdAsync(int id);
        Task UpdateProjectPlanAsync(ProjectPlan projectPlan);
        Task DeleteProjectPlanAsync(int id);
        Task<IEnumerable<ProjectPlan>> GetPagedProjectPlansAsync(string search, int pageNumber, int pageSize);
        Task<IEnumerable<ProjectPlan>> GetProjectPlansAsync(string search);
        Task ChangeProjectPlanStatusAsync(int id, string status);
    }

}
