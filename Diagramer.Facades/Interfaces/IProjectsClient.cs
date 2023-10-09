using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Project;

namespace Diagramer.Facades.Interfaces;

public interface IProjectsClient
{
    public Result<ProjectDefinition> CreateNewProject(string projectName, List<string> paths);
}