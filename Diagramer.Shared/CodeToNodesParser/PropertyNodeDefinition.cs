using Diagramer.SharedModels.CodeToNodesParser.Core;

namespace Diagramer.SharedModels.CodeToNodesParser;

public class PropertyNodeDefinition : IBasicNodeDataDefinition
{
    public string Name { get; set; }
    public string DataType { get; set; }
    public List<string> Modifiers { get; set; }
    public bool IsGetterOnly { get; set; }

    public PropertyNodeDefinition()
    {
        Name = "";
        DataType = "";
        Modifiers = new List<string>();
        IsGetterOnly = false;
    }
}
