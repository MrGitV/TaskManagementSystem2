using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    // Service for managing project-related operations.
    public class ProjectService(AppDbContext context) : IProjectService
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        // Retrieves all projects from the database.
        public async Task<List<ProjectResponse>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects.AsNoTracking().Include(p => p.Tasks).ToListAsync();
            return [.. projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description
            })];
        }

        // Creates a new project.
        public async Task<bool> CreateProjectAsync(CreateProjectRequest request)
        {
            var project = new Project
            {
                Title = request.Title,
                Description = request.Description
            };
            _context.Projects.Add(project);
            return await _context.SaveChangesAsync() > 0;
        }

        // Deletes a project by its ID.
        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return false;
            _context.Projects.Remove(project);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}