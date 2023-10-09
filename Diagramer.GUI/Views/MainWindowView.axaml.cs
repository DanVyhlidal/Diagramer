using Avalonia.ReactiveUI;
using Diagramer.GUI.ViewModels.Core;
using ReactiveUI;
using Splat;

namespace Diagramer.GUI.Views;

public partial class MainWindowView : ReactiveWindow<MainWindowViewModel>
{
    public MainWindowView()
    {
        DataContext = Locator.Current.GetService<MainWindowViewModel>();
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}