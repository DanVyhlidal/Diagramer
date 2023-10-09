using ReactiveUI;

namespace Diagramer.GUI.Models;

public class ProjectReactiveFile : ReactiveObject
{
    private string fileName;
    private string filePath;
    private bool isSelected;
    private bool isInSearch;
    
    public string FilePath { get => filePath; set => filePath = value; }

    public string FileName
    {
        get
        {
            string[] pathParts = filePath.Split('\\');
            return pathParts[pathParts.Length - 1];
        }
    }

    public bool IsInSearch
    {
        get => isInSearch;
        set => this.RaiseAndSetIfChanged(ref isInSearch, value);
    }
    
    public bool IsSelected
    {
        get => isSelected;
        set => this.RaiseAndSetIfChanged(ref isSelected, value);
    }
}