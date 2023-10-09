using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Collections;
using Diagramer.GUI.Models;
using Diagramer.GUI.ViewModels.Core;
using Diagramer.GUI.ViewModels.Core.Interfaces;
using Diagramer.GUI.ViewModels.EntityMappers;
using Diagramer.Models;
using Diagramer.Services.Projects.Core;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Project;
using Diagramer.SharedModels.Settings;
using ReactiveUI;

namespace Diagramer.GUI.ViewModels;

public class HomeViewModel : BaseViewModel<HomeViewModelMapper>
{
    private readonly ISettingsService settingsService;
    private readonly IProjectsService projectsService;

    private string textBoxSearch = String.Empty;

    public ObservableCollection<ProjectReactiveDefinition> Projects { get; }
    private DataGridCollectionView projectsToDisplay;

    public DataGridCollectionView ProjectsToDisplay
    {
        get => projectsToDisplay;
        set { this.RaiseAndSetIfChanged(ref projectsToDisplay, value); }
    }

    public ObservableCollection<ModifierReactiveDefinition> MemberAccessibilityModifiers { get; } = new();
    public ObservableCollection<ModifierReactiveDefinition> MemberModifiers { get; } = new();
    public ObservableCollection<ModifierReactiveDefinition> TypeKeywords { get; } = new();
    public ObservableCollection<ModifierReactiveDefinition> TypeModifiers { get; } = new();

    public ProjectReactiveDefinition SelectedProject { get; set; }

    public string TextboxSearch
    {
        get => textBoxSearch;
        set
        {
            this.RaiseAndSetIfChanged(ref textBoxSearch, value);

            ProjectsToDisplay = new DataGridCollectionView(Projects);
            ProjectsToDisplay.Filter = FilterProjectsByName;
            ProjectsToDisplay.Refresh();
        }
    }

    public ReactiveCommand<Unit, Unit> SaveSettings => ReactiveCommand.Create(RunSaveSettings);
    public ReactiveCommand<Unit, Unit> OpenNewProject => ReactiveCommand.Create(RunOpenNewProject);

    public HomeViewModel(IScreen screen,
        IPageNavigator pageNavigator,
        ISettingsService settingsService,
        IProjectsService projectsService
    ) : base(screen, pageNavigator)
    {
        this.settingsService = settingsService;
        this.projectsService = projectsService;
        InitializeSettings();
        Projects = new ObservableCollection<ProjectReactiveDefinition>();
    }

    private void InitializeSettings()
    {
        List<ModifierDefinition> memberAccessModifiers = settingsService.GetMemberAccessModifiers();
        memberAccessModifiers.ForEach(x => MemberAccessibilityModifiers.Add(modelMapper.MapEntity(x)));

        List<ModifierDefinition> memberModifiers = settingsService.GetMemberModifiers();
        memberModifiers.ForEach(x => MemberModifiers.Add(modelMapper.MapEntity(x)));

        List<ModifierDefinition> typeKeywords = settingsService.GetTypeKeywords();
        typeKeywords.ForEach(x => TypeKeywords.Add(modelMapper.MapEntity(x)));

        List<ModifierDefinition> typeModifiers = settingsService.GetTypeModifiers();
        typeModifiers.ForEach(x => TypeModifiers.Add(modelMapper.MapEntity(x)));
    }

    private void InitializeProjects()
    {
        Projects.Clear();

        Result<List<(int, ProjectDefinition)>> getAllProjectsResult = projectsService.GetAllProjects();
        if (getAllProjectsResult.HasError)
        {
            // show error
            return;
        }

        getAllProjectsResult.ResultObject.ForEach(x =>
        {
            ProjectReactiveDefinition mappedProject = modelMapper.MapEntity(x.Item2, x.Item1);
            Projects.Add(mappedProject);
        });

        ProjectsToDisplay = new DataGridCollectionView(Projects);
        ProjectsToDisplay.Filter = FilterProjectsByName;
    }

    #region Commands

    private async void RunSaveSettings()
    {
        List<Task> tasks = new List<Task>();

        List<ModifierDefinition> modifiers = new List<ModifierDefinition>();
        MemberAccessibilityModifiers.ToList().ForEach(x => modifiers.Add(modelMapper.DemapEntity(x)));
        tasks.Add(settingsService.UpdateMemberAccessModifiers(modifiers));

        modifiers = new List<ModifierDefinition>();
        MemberModifiers.ToList().ForEach(x => modifiers.Add(modelMapper.DemapEntity(x)));
        tasks.Add(settingsService.UpdateMemberModifiers(modifiers));

        modifiers = new List<ModifierDefinition>();
        TypeKeywords.ToList().ForEach(x => modifiers.Add(modelMapper.DemapEntity(x)));
        tasks.Add(settingsService.UpdateTypeKeywords(modifiers));

        modifiers = new List<ModifierDefinition>();
        TypeModifiers.ToList().ForEach(x => modifiers.Add(modelMapper.DemapEntity(x)));
        tasks.Add(settingsService.UpdateTypeModifiers(modifiers));

        await Task.WhenAll(tasks);
    }

    private void RunOpenNewProject()
    {
        pageNavigator.Navigate<NewProjectViewModel>();
    }

    public void RunOpenProject()
    {
        pageNavigator.Navigate<ProjectViewModel, int>(SelectedProject.Id);
    }

    #endregion

    public override void OnSwitch()
    {
        InitializeProjects();
    }

    private bool FilterProjectsByName(object arg)
    {
        if (TextboxSearch != string.Empty)
        {
            var projectDetail = arg as ProjectReactiveDefinition;
            return projectDetail != null && projectDetail.Name.Contains(TextboxSearch);
        }

        return true;
    }
}