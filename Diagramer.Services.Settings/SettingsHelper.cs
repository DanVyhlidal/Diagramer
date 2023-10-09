using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.Settings;

namespace Diagramer.Services.Settings;

public class SettingsHelper : ISettingsHelper
{
    private readonly ISettingsService settingsService;
    
    public SettingsHelper(ISettingsService settingsService)
    {
        this.settingsService = settingsService;
    }
    
    public string FindMemberModifier(string modifier)
    {
        List<ModifierDefinition> settingsModifiers = settingsService.GetMemberModifiers();
        
        foreach (ModifierDefinition modifierDefinition in settingsModifiers)
        {
            if (modifierDefinition.OriginalName.Equals(modifier))
            {
                return modifierDefinition.ModifiedName;
            }
        }
        
        settingsModifiers = settingsService.GetMemberAccessModifiers();
        foreach (ModifierDefinition modifierDefinition in settingsModifiers)
        {
            if (modifierDefinition.OriginalName.Equals(modifier))
            {
                return modifierDefinition.ModifiedName;
            }
        }

        return string.Empty;
    }

    public string FindTypeModifier(string modifier)
    {
        List<ModifierDefinition> settingsModifiers = settingsService.GetTypeModifiers();
        foreach (ModifierDefinition modifierDefinition in settingsModifiers)
        {
            if (modifierDefinition.OriginalName.Equals(modifier))
            {
                return modifierDefinition.ModifiedName;
            }
        }

        return string.Empty;
    }

    public string FindTypeKeyword(string keyword)
    {
        List<ModifierDefinition> keywordModifiers = settingsService.GetTypeKeywords();
        
        foreach (ModifierDefinition modifierDefinition in keywordModifiers)
        {
            if (modifierDefinition.OriginalName.Equals(keyword))
            {
                return modifierDefinition.ModifiedName;
            }
        }

        return string.Empty;
    }
}