using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Diagram.Services.Exporters;
using Diagram.Services.Exporters.Core;
using Diagramer.GUI;
using Diagramer.GUI.ViewModels;
using Diagramer.GUI.ViewModels.Core;
using Diagramer.GUI.ViewModels.Core.Interfaces;
using Diagramer.GUI.Views;
using Diagramer.Infrastructure.CodeParsers.RoslynConvertor;
using Diagramer.Infrastructure.DiagramParsers.PlantUML;
using Diagramer.Infrastructure.Exporters;
using Diagramer.Infrastructure.Extensions;
using Diagramer.Repositories.Core.BaseData;
using Diagramer.Repositories.Core.Serializers;
using Diagramer.Repositories.Projects;
using Diagramer.Repositories.Projects.Interfaces;
using Diagramer.Repositories.Settings;
using Diagramer.Repositories.Settings.Interfaces;
using Diagramer.Services.CodeParser;
using Diagramer.Services.CodeParser.Core;
using Diagramer.Services.Diagrams;
using Diagramer.Services.Diagrams.Core;
using Diagramer.Services.Projects;
using Diagramer.Services.Projects.Core;
using Diagramer.Services.Settings;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.Project;
using Diagramer.SharedModels.Settings;
using ReactiveUI;
using Splat;

namespace Diagramer.Desktop;

class Program
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    [STAThread]
    public static void Main(string[] args)
    {
        using (Mutex mutex = new Mutex(true, "Diagramer",out var createdNew))
        {
            if (createdNew)
            {
                RegisterDependencies();
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            else
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {

                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
            }
        }
    }

    private static void RegisterDependencies()
    {
        IMutableDependencyResolver services = Locator.CurrentMutable;
        IReadonlyDependencyResolver resolver = Locator.Current;

        RepositoryRegistration(services, resolver);
        ServiceRegistration(services, resolver);
        ViewModelRegistration(services, resolver);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();

    private static void RepositoryRegistration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        // Settings
        services.RegisterLazySingleton<IMemberAccessibilityModifiersRepository>(() => new MemberAccessibilityModifiersRepository(
            new JsonSerializer<ModifierDefinition>(
                "MemberAccessibilityModifiers", 
                SettingsBaseDataGenerator.CreateMemberAccessibilityModifiers())));
        
        services.RegisterLazySingleton<IMemberModifiersRepository>(() => new MemberModifiersRepository(
            new JsonSerializer<ModifierDefinition>(
                "MemberModifiers",
                SettingsBaseDataGenerator.CreateMemberModifiers())));
        
        services.RegisterLazySingleton<ITypeKeywordsRepository>(() => new TypeKeywordsRepository(
            new JsonSerializer<ModifierDefinition>(
                "TypeKeywords",
                SettingsBaseDataGenerator.CreateTypeKeywords())));
        
        services.RegisterLazySingleton<ITypeModifiersRepository>(() => new TypeModifiersRepository(
            new JsonSerializer<ModifierDefinition>(
                "TypeModifiers",
                SettingsBaseDataGenerator.CreateTypeModifiers())));
        
        // Projects
        services.RegisterLazySingleton<IProjectsRepository>(() => new ProjectsRepository(
            new JsonSerializer<ProjectDefinition>("Projects")));
    }

    private static void ServiceRegistration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ISettingsService>(() => new SettingsService(
            resolver.GetRequiredService<IMemberAccessibilityModifiersRepository>(),
            resolver.GetRequiredService<IMemberModifiersRepository>(),
            resolver.GetRequiredService<ITypeKeywordsRepository>(),
            resolver.GetRequiredService<ITypeModifiersRepository>()));
        
        services.RegisterLazySingleton<ISettingsHelper>(() => new SettingsHelper(
            resolver.GetRequiredService<ISettingsService>()));
        
        services.RegisterLazySingleton<IProjectsService>(() => new ProjectsService(
            resolver.GetRequiredService<IProjectsRepository>()));
        
        services.RegisterLazySingleton<ICodeParserService>(() => new CodeParserService<RoslynParser>());
        
        services.RegisterLazySingleton<IDiagramService>(() => new DiagramService<PlantUmlParser>(
            resolver.GetRequiredService<ICodeParserService>(),
            resolver.GetRequiredService<ISettingsHelper>()));
        
        services.RegisterLazySingleton<IExportService>(() => new ExportService<PlantUmlExporter>(
            "https://localhost:7102/"));
    }

    private static void ViewModelRegistration(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<Window>(() => new MainWindowView());
        services.RegisterLazySingleton<MainWindowViewModel>(() => new MainWindowViewModel());
        
        services.RegisterLazySingleton<IScreen>(() => resolver.GetRequiredService<MainWindowViewModel>());
        services.RegisterLazySingleton<IPageNavigator>(() => resolver.GetRequiredService<MainWindowViewModel>());

        services.RegisterLazySingleton<HomeViewModel>(() => new HomeViewModel(
            resolver.GetRequiredService<IScreen>(),
            resolver.GetRequiredService<IPageNavigator>(),
            resolver.GetRequiredService<ISettingsService>(),
            resolver.GetRequiredService<IProjectsService>()));
        
        services.RegisterLazySingleton<NewProjectViewModel>(() => new NewProjectViewModel(
            resolver.GetRequiredService<IScreen>(),
            resolver.GetRequiredService<IPageNavigator>(),
            resolver.GetRequiredService<IProjectsService>()));
        
        services.RegisterLazySingleton<ProjectViewModel>(() => new ProjectViewModel(
            resolver.GetRequiredService<IScreen>(),
            resolver.GetRequiredService<IPageNavigator>(),
            resolver.GetRequiredService<IProjectsService>(),
            resolver.GetRequiredService<IDiagramService>(),
            resolver.GetRequiredService<IExportService>()));
    }
    
}
