using Diagramer.SharedModels.Settings;

namespace Diagramer.Repositories.Core.BaseData;

public static class SettingsBaseDataGenerator
{
    public static List<ModifierDefinition> CreateMemberAccessibilityModifiers()
    {
        var modifierDefinitions = new List<ModifierDefinition>();
        
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "public", ModifiedName = "+" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "private", ModifiedName = "-" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "protected", ModifiedName = "#", });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "internal", ModifiedName = "<<internal>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "protected internal", ModifiedName = "# <<internal>>" });

        return modifierDefinitions;
    }
    
    public static List<ModifierDefinition> CreateMemberModifiers()
    {
        var modifierDefinitions = new List<ModifierDefinition>();
        
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "abstract", ModifiedName = "{abstract}" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "static", ModifiedName = "{static}" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "virtual", ModifiedName = "<<virtual>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "override", ModifiedName = "<<override>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "new", ModifiedName = "<<new>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "readonly", ModifiedName = "<<readonly>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "event", ModifiedName = "<<event>>" });
        
        return modifierDefinitions;
    }
    
    public static List<ModifierDefinition> CreateTypeKeywords()
    {
        var modifierDefinitions = new List<ModifierDefinition>();
        
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "class", ModifiedName = "class" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "struct", ModifiedName = "struct" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "enum", ModifiedName = "enum" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "interface", ModifiedName = "interface" });

        return modifierDefinitions;
    }
    
    public static List<ModifierDefinition> CreateTypeModifiers()
    {
        var modifierDefinitions = new List<ModifierDefinition>();
        
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "abstract", ModifiedName = "<<abstract>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "static", ModifiedName = "<<static>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "partial", ModifiedName = "<<partial>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "sealed", ModifiedName = "<<sealed>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "internal", ModifiedName = "<<internal>>" });
        modifierDefinitions.Add(new ModifierDefinition() { OriginalName = "MonoBehaviour", ModifiedName = "<<MonoBehaviour>>" });

        return modifierDefinitions;
    }
}