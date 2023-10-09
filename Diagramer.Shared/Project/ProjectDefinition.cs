using Diagramer.SharedModels.Core;

namespace Diagramer.SharedModels.Project;

public class ProjectDefinition : ISerializableDefinition
{
    public List<string> FilePaths { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public ProjectDefinition()
    {
        FilePaths = new List<string>();
        Name = string.Empty;
        Type = string.Empty;
    }
}