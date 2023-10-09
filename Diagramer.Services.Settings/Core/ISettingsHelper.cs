namespace Diagramer.Services.Settings.Core;

public interface ISettingsHelper
{
    public string FindMemberModifier(string modifier);
    public string FindTypeModifier(string modifier);
    public string FindTypeKeyword(string keyword);
}