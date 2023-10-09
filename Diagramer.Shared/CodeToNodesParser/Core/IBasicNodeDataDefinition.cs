namespace Diagramer.SharedModels.CodeToNodesParser.Core;

public interface IBasicNodeDataDefinition
{
    public string Name { get; set; }
    public string DataType { get; set; }

    public List<string> Modifiers { get; set; }
}