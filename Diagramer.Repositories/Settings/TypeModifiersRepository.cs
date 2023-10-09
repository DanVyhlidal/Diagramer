using Diagramer.Repositories.Core;
using Diagramer.Repositories.Core.Serializers;
using Diagramer.Repositories.Settings.Interfaces;
using Diagramer.SharedModels.Settings;

namespace Diagramer.Repositories.Settings;

public class TypeModifiersRepository : ARepository<ModifierDefinition>, ITypeModifiersRepository
{
    public TypeModifiersRepository(JsonSerializer<ModifierDefinition> dataSerializer) : base(dataSerializer)
    {
        
    }
}