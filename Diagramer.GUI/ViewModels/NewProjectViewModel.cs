using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Diagramer.GUI.Models;
using Diagramer.GUI.ViewModels.Core;
using Diagramer.GUI.ViewModels.Core.Interfaces;
using Diagramer.GUI.ViewModels.EntityMappers;
using Diagramer.Services.Projects.Core;
using Diagramer.SharedModels.Core;
using DynamicData;
using ReactiveUI;
using Splat;

namespace Diagramer.GUI.ViewModels;

public class NewProjectViewModel : BaseViewModel<NewProjectViewModelMapper>
{
    private readonly IProjectsService projectsService;

    public ReactiveCommand<Unit, Unit> SelectFiles =>
        ReactiveCommand.CreateFromTask(async () => await RunSelectFiles());

    public ReactiveCommand<Unit, Unit> OpenHomePage => ReactiveCommand.Create(RunOpenHomePage);
    public ReactiveCommand<Unit, Unit> CreateNewProject => ReactiveCommand.Create(RunCreateNewProject);

    public ProjectReactiveDefinition Project { get; } = new();

    private OpenFileDialog openFileDialog;

    public NewProjectViewModel(
        IScreen screen,
        IPageNavigator pageNavigator,
        IProjectsService projectsService) : base(screen, pageNavigator)
    {
        this.projectsService = projectsService;
        InitializeOpenFileDialog();
    }

    private void InitializeOpenFileDialog()
    {
        openFileDialog = new OpenFileDialog();

        var filter = new FileDialogFilter()
        {
            Name = ".cs",
            Extensions = { "cs", "csproj" }
        };

        openFileDialog.Title = "Select C# project files";
        openFileDialog.AllowMultiple = true;
        openFileDialog.Filters.Add(filter);
    }

    public async Task RunSelectFiles()
    {
        Window mainWindow = Locator.Current.GetService<Window>();
        string[]? result = await openFileDialog.ShowAsync(mainWindow);

        if (result == null)
        {
            return;
        }

        Project.FilePaths.AddRange(result.ToList());
    }

    private void RunOpenHomePage()
    {
        pageNavigator.Navigate<HomeViewModel>();
    }

    private async void RunCreateNewProject()
    {
        if (Project.NumberOfFiles == 0)
        {
            // Show dialog --> Select files
            return;
        }

        Result<int> createNewProjectResult = await projectsService.CreateNewProject(modelMapper.DemapEntity(Project));

        if (createNewProjectResult.HasError)
        {
            // print out the error
            return;
        }

        pageNavigator.Navigate<ProjectViewModel, int>(createNewProjectResult.ResultObject);
    }

    public override void OnSwitch()
    {
    }
}