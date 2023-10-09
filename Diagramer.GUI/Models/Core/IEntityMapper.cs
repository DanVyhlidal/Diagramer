using Diagramer.SharedModels.Core;
using ReactiveUI;

namespace Diagramer.GUI.Models.Core;

public interface IEntityMapper<TEntityService, TEntityUi> : IMapper
    where TEntityService : ISerializableDefinition, new()
    where TEntityUi : ReactiveObject, new()
{
    TEntityUi MapEntity(TEntityService mapEntity);
    TEntityService DemapEntity(TEntityUi mapEntity);
}

public interface IEntityMapperWithId<TEntityService, TEntityUi> : IMapper
    where TEntityService : ISerializableDefinition, new()
    where TEntityUi : ReactiveObject, new()
{
    TEntityUi MapEntity(TEntityService mapEntity, int id);
    TEntityService DemapEntity(TEntityUi mapEntity);
}