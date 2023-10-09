using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Project;

namespace Diagramer.Services.Projects.Core;

public interface IProjectsService
{
    Task<Result<int>> CreateNewProject(ProjectDefinition projectDefinition);
    Result<List<(int, ProjectDefinition)>> GetAllProjects();
    Result<ProjectDefinition> GetProject(int id);
}