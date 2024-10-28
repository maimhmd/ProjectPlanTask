using Microsoft.EntityFrameworkCore;
using OneTrack.PM.Core.Repositories;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using System.Threading.Tasks;

namespace OneTrack.PM.Repositories
{
    public class ProjectPlanRepository : IProjectPlanRepository
    {
        private readonly OneTrackPMContext _context;

        public ProjectPlanRepository(OneTrackPMContext context)
        {
            _context = context;
        }

        public async Task<ProjectPlan> CreateProjectPlanAsync(ProjectPlan projectPlan)
        {
            // Add the project plan to the database context
            _context.ProjectPlans.Add(projectPlan);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created project plan
            return projectPlan;
        }
        public async Task<ProjectPlan> GetProjectPlanByIdAsync(int id)
        {
            return await _context.ProjectPlans.FindAsync(id);
        }

        public async Task UpdateProjectPlanAsync(ProjectPlan projectPlan)
        {
            _context.ProjectPlans.Update(projectPlan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectPlanAsync(int id)
        {
            var projectPlan = await _context.ProjectPlans.FindAsync(id);
            if (projectPlan != null)
            {
                _context.ProjectPlans.Remove(projectPlan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProjectPlan>> GetPagedProjectPlansAsync(string search, int pageNumber, int pageSize)
        {
            return await _context.ProjectPlans
                .Where(p => p.Name.Contains(search) || p.Description.Contains(search))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectPlan>> GetProjectPlansAsync(string search)
        {
            return await _context.ProjectPlans
                .Where(p => p.Name.Contains(search) || p.Description.Contains(search))
                .ToListAsync();
        }
    }


}

