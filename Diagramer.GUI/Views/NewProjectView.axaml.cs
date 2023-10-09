using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Diagramer.GUI.ViewModels;
using ReactiveUI;

namespace Diagramer.GUI.Views;

public partial class NewProjectView : ReactiveUserControl<NewProjectViewModel>
{
    public NewProjectView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}