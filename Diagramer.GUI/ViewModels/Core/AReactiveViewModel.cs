using ReactiveUI;

namespace Diagramer.GUI.ViewModels.Core;

public abstract class AReactiveViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; set; }
    public IScreen HostScreen { get; set; }

    public abstract void OnSwitch();
}