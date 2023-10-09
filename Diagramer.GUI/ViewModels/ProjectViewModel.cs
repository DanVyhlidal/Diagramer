using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Diagram.Services.Exporters.Core;
using Diagramer.GUI.Models;
using Diagramer.GUI.Models.Enums;
using Diagramer.GUI.ViewModels.Core;
using Diagramer.GUI.ViewModels.Core.Interfaces;
using Diagramer.GUI.ViewModels.EntityMappers;
using Diagramer.Services.Diagrams;
using Diagramer.Services.Diagrams.Core;
using Diagramer.Services.Projects.Core;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Project;
using ReactiveUI;
using Splat;

namespace Diagramer.GUI.ViewModels;

public class ProjectViewModel : BaseViewModel<ProjectViewModelMapper, int>
{
    private readonly IProjectsService projectsService;
    private readonly IDiagramService diagramService;
    private readonly IExportService exportService;

    private SaveFileDialog saveFileDialog;

    private string searchBarText = string.Empty;
    private ObservableCollection<ProjectReactiveFile> files;

    public string SearchBarText
    {
        get => searchBarText;
        set
        {
            searchBarText = value;
            OnDiagramSearch();
        }
    }

    public ProjectReactiveDefinition Project { get; set; }

    public ObservableCollection<ProjectReactiveFile> Files
    {
        get => files;
        set => this.RaiseAndSetIfChanged(ref files, value);
    }

    public DiagramReactiveImage DiagramImage { get; } = new();
    public int SelectedDiagram { get; set; }


    public ReactiveCommand<Unit, Unit> OpenHomePage => ReactiveCommand.Create(RunOpenHomePage);
    public ReactiveCommand<Unit, Unit> GenerateDiagram => ReactiveCommand.Create(RunGenerateDiagram);
    public ReactiveCommand<Unit, Unit> ExportDiagram => ReactiveCommand.Create(RunExportDiagram);
    public ReactiveCommand<Unit, Unit> ShareDiagram => ReactiveCommand.Create(RunShareDiagram);

    public ProjectViewModel(
        IScreen screen,
        IPageNavigator pageNavigator,
        IProjectsService projectsService,
        IDiagramService diagramService,
        IExportService exportService) : base(screen, pageNavigator)
    {
        this.projectsService = projectsService;
        this.diagramService = diagramService;
        this.exportService = exportService;

        DiagramImage.IsImageEnabled = false;
        Files = new ObservableCollection<ProjectReactiveFile>();
        InitializeSaveFileDialog();
    }

    public override void OnSwitch()
    {
        Files.Clear();
        Result<ProjectDefinition> getProjectResult = projectsService.GetProject(Parameter);

        if (getProjectResult.HasError)
        {
            // print out the error
            return;
        }

        Project = modelMapper.MapEntity(getProjectResult.ResultObject);

        getProjectResult.ResultObject.FilePaths.ForEach(x => Files.Add(new ProjectReactiveFile()
        {
            FilePath = x,
            IsSelected = true,
            IsInSearch = true
        }));
    }

    #region Commands

    private void RunOpenHomePage()
    {
        pageNavigator.Navigate<HomeViewModel>();
    }

    private async void RunGenerateDiagram()
    {
        string diagram = GetDiagram();

        if (diagram == string.Empty)
        {
            return;
        }

        Result<byte[]> getBytesResult = await exportService.ExportToImageBytes(diagram);

        if (getBytesResult.HasError)
        {
            // print out the error
            return;
        }

        DiagramImage.ImageData = getBytesResult.ResultObject;
        DiagramImage.IsImageEnabled = true;

        DiagramImage.ImageSource = Encoding.UTF8.GetString(DiagramImage.ImageData);
    }


    private async void RunExportDiagram()
    {
        if (!DiagramImage.IsReadyForExport)
        {
            //TODO: Print out that they should generate the image first
            return;
        }

        Window mainWindow = Locator.Current.GetService<Window>();

        string pathResult = await saveFileDialog.ShowAsync(mainWindow);

        await exportService.ExportToSvg(pathResult, DiagramImage.ImageData);
    }

    private async void RunShareDiagram()
    {
        if (!DiagramImage.IsReadyForExport)
        {
            return;
        }

        Result<string> linkResult = await exportService.ShareDiagram(DiagramImage.ImageData);

        if (linkResult.HasError)
        {
            //TODO write out the connection problems error
            return;
        }

        if (linkResult.ResultObject == string.Empty)
        {
            // TODO show error dialog
            return;
        }

        await Application.Current.Clipboard.SetTextAsync(linkResult.ResultObject);
    }

    #endregion

    private string GetDiagram()
    {
        ProjectDefinition selectedProject = modelMapper.DemapEntity(Project);
        var selectedFiles = Files.Where(x => x.IsSelected == true).Select(x => x.FilePath).ToList();
        if (SelectedDiagram == (int)DiagramTypes.ClassDiagram)
        {
            Result<string> classDiagramResult = diagramService.GenerateClassDiagram(selectedFiles);

            if (classDiagramResult.HasError)
            {
                //TODO: print error
                return string.Empty;
            }

            return classDiagramResult.ResultObject;
        }

        if (SelectedDiagram == (int)DiagramTypes.DependencyDiagram)
        {
            Result<string> dependencyDiagramResult =
                diagramService.GenerateDependencyDiagram(selectedFiles);

            if (dependencyDiagramResult.HasError)
            {
                //TODO: print error
                return string.Empty;
            }

            return dependencyDiagramResult.ResultObject;
        }

        if (SelectedDiagram == (int)DiagramTypes.IndividualDiagram)
        {
            if (selectedFiles.Count != 1)
            {
                // TODO: Say that the selected file has to be 1
                return string.Empty;
            }

            Result<string> individualDiagramResult =
                diagramService.GenerateIndividualDiagram(selectedFiles.FirstOrDefault());

            if (individualDiagramResult.HasError)
            {
                //TODO: print error
                return string.Empty;
            }

            return individualDiagramResult.ResultObject;
        }

        return string.Empty;
    }

    private void InitializeSaveFileDialog()
    {
        saveFileDialog = new SaveFileDialog();

        var filter = new FileDialogFilter()
        {
            Name = ".svg",
            Extensions = { "svg" }
        };

        saveFileDialog.Title = "Save diagram as .svg";
        saveFileDialog.Filters.Add(filter);
    }

    private void OnDiagramSearch()
    {
        string searchText = SearchBarText;
        if (searchText == string.Empty)
        {
            Files.ToList().ForEach(x => x.IsInSearch = true);
        }

        searchText = searchText.ToLower();

        Files.ToList().ForEach(x => { x.IsInSearch = x.FileName.ToLower().Contains(searchText); });
    }
}