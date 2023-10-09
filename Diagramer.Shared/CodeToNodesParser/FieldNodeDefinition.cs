using Diagramer.SharedModels.CodeToNodesParser.Core;

namespace Diagramer.SharedModels.CodeToNodesParser;

public class FieldNodeDefinition : IBasicNodeDataDefinition
{
    public string Name { get; set; }
    public string DataType { get; set; }
    public List<string> Modifiers { get; set; }

    public FieldNodeDefinition()
    {
        Name = "";
        DataType = "";
        Modifiers = new List<string>();
    }
}
