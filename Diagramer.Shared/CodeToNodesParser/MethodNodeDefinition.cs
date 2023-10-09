using Diagramer.SharedModels.CodeToNodesParser.Core;

namespace Diagramer.SharedModels.CodeToNodesParser;

public class MethodNodeDefinition : IBasicNodeDataDefinition
{
    public string Name { get; set; }
    public string DataType { get; set; }
    public List<string> Modifiers { get; set; }
    public Dictionary<string, string> Arguments { get; set; }

    public MethodNodeDefinition()
    {
        Name = "";
        DataType = "";
        Modifiers = new List<string>();
        Arguments = new Dictionary<string, string>();
    }
}
