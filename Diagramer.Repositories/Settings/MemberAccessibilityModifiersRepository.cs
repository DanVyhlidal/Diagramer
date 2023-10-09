using Diagramer.Repositories.Core;
using Diagramer.Repositories.Core.Serializers;
using Diagramer.Repositories.Settings.Interfaces;
using Diagramer.SharedModels.Settings;

namespace Diagramer.Repositories.Settings;

public class MemberAccessibilityModifiersRepository : ARepository<ModifierDefinition>, IMemberAccessibilityModifiersRepository
{
    public MemberAccessibilityModifiersRepository(JsonSerializer<ModifierDefinition> dataSerializer) : base(dataSerializer)
    {
    }
}