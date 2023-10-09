using System.Collections.ObjectModel;
using System.Linq;
using Diagramer.GUI.Models;
using Diagramer.GUI.Models.Core;
using Diagramer.Models;
using Diagramer.SharedModels.Project;
using Diagramer.SharedModels.Settings;

namespace Diagramer.GUI.ViewModels.EntityMappers;

public class HomeViewModelMapper : 
    IEntityMapper<ModifierDefinition, ModifierReactiveDefinition>,
    IEntityMapperWithId<ProjectDefinition, ProjectReactiveDefinition>
{
    public ModifierReactiveDefinition MapEntity(ModifierDefinition mapEntity) =>
        new ()
        {
            ModifiedName = mapEntity.ModifiedName,
            OriginalName = mapEntity.OriginalName
        };
    
    public ModifierDefinition DemapEntity(ModifierReactiveDefinition mapEntity) =>
        new ()
        {
            ModifiedName = mapEntity.ModifiedName,
            OriginalName = mapEntity.OriginalName
        };

    public ProjectReactiveDefinition MapEntity(ProjectDefinition mapEntity, int id) =>
        new ()
        {
            Id = id,
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