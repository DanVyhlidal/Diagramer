using Diagramer.SharedModels.Core;

namespace Diagramer.SharedModels.Settings;

public class ModifierDefinition : ISerializableDefinition
{
    public string OriginalName { get; set; }
    public string ModifiedName { get; set; }

    public ModifierDefinition()
    {
        OriginalName = "";
        ModifiedName = "";
    }
}