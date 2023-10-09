using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Settings;

namespace Diagramer.Services.Settings.Core;

public interface ISettingsService
{
    List<ModifierDefinition> GetMemberAccessModifiers();
    Task<Result<bool>> UpdateMemberAccessModifiers(List<ModifierDefinition> memberAccessModifiers);
    List<ModifierDefinition> GetMemberModifiers();
    Task<Result<bool>> UpdateMemberModifiers(List<ModifierDefinition> memberAccessModifiers);
    List<ModifierDefinition> GetTypeKeywords();
    Task<Result<bool>> UpdateTypeKeywords(List<ModifierDefinition> memberAccessModifiers);
    List<ModifierDefinition> GetTypeModifiers();
    Task<Result<bool>> UpdateTypeModifiers(List<ModifierDefinition> memberAccessModifiers);
}