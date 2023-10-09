using Diagramer.Repositories.Core;
using Diagramer.Repositories.Core.Serializers;
using Diagramer.Repositories.Projects.Interfaces;
using Diagramer.SharedModels.Project;

namespace Diagramer.Repositories.Projects;

public class ProjectsRepository : ARepository<ProjectDefinition>, IProjectsRepository
{
    public ProjectsRepository(JsonSerializer<ProjectDefinition> dataSerializer) : base(dataSerializer)
    {
        
    }
}