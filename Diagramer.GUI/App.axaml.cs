using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Diagramer.GUI.ViewModels.Core;
using Splat;

namespace Diagramer.GUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Locator.Current.GetService<Window>();
        }
        
        Locator.Current.GetService<MainWindowViewModel>().OnInitilizationComplete();

        base.OnFrameworkInitializationCompleted();
    }
}