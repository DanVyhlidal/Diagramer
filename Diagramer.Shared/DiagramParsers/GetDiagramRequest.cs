using Diagramer.SharedModels.CodeToNodesParser;

namespace Diagramer.SharedModels.DiagramParsers;

public class GetDiagramRequest
{
    public List<TypeNodeDefinition> TypeNodes { get; set; }
    public Dictionary<string, List<DependencyDefinition>> Dependencies { get; set; }
}