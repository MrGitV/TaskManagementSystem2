using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApp.DTOs;

namespace ClientApp.Services
{
    // Defines the contract for a service that manages projects.
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
    }
}