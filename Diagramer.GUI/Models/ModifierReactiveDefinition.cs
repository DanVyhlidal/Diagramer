using ReactiveUI;

namespace Diagramer.Models;

public class ModifierReactiveDefinition : ReactiveObject
{
    private string originalName;
    private string modifiedName;
    
    public string OriginalName {get => originalName; set => originalName = value; }
    public string ModifiedName {get => modifiedName; set => modifiedName = value; }
}