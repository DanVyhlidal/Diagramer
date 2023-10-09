using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Diagramer.GUI.ViewModels;
using ReactiveUI;

namespace Diagramer.GUI.Views;

public partial class ProjectView : ReactiveUserControl<ProjectViewModel>
{
    public ProjectView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}