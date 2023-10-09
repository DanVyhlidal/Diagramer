using System.Collections.ObjectModel;
using ReactiveUI;

namespace Diagramer.GUI.Models;

public class ProjectReactiveDefinition : ReactiveObject
{
    private int id;
    private ObservableCollection<string> filePaths = new();
    private string name;
    private string type;
    
    public int Id { get => id; set => id = value; }
    public ObservableCollection<string> FilePaths { get => filePaths; set => this.RaiseAndSetIfChanged(ref filePaths, value); }
    public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }
    public string Type { get => type; set => type = value; }
    public int NumberOfFiles => filePaths.Count;
}