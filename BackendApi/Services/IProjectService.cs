using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Services
{
    // Interface for project CRUD.
    public interface IProjectService
    {
        Task<List<ProjectResponse>> GetAllProjectsAsync();
        Task<bool> CreateProjectAsync(CreateProjectRequest request);
        Task<bool> DeleteProjectAsync(int projectId);
    }
}