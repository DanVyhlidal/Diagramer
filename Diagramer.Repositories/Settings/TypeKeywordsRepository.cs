using Diagramer.Repositories.Core;
using Diagramer.Repositories.Core.Serializers;
using Diagramer.Repositories.Settings.Interfaces;
using Diagramer.SharedModels.Settings;

namespace Diagramer.Repositories.Settings;

public class TypeKeywordsRepository : ARepository<ModifierDefinition>, ITypeKeywordsRepository
{
    public TypeKeywordsRepository(JsonSerializer<ModifierDefinition> dataSerializer) : base(dataSerializer)
    {
    }
}