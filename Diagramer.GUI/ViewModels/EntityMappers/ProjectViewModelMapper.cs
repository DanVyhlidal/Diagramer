using System.Collections.ObjectModel;
using System.Linq;
using Diagramer.GUI.Models;
using Diagramer.GUI.Models.Core;
using Diagramer.SharedModels.Project;

namespace Diagramer.GUI.ViewModels.EntityMappers;

public class ProjectViewModelMapper : IEntityMapper<ProjectDefinition, ProjectReactiveDefinition>
{
    public ProjectReactiveDefinition MapEntity(ProjectDefinition mapEntity) =>
        new ()
        {
            Name = mapEntity.Name,
            Type = mapEntity.Type,
            FilePaths = new ObservableCollection<string>(mapEntity.FilePaths)
        };

    public ProjectDefinition DemapEntity(ProjectReactiveDefinition mapEntity) =>
        new ()
        {
            Name = mapEntity.Name,
            Type = mapEntity.Type,
            FilePaths = mapEntity.FilePaths.ToList()
        };
}