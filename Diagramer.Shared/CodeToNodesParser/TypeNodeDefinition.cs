namespace Diagramer.SharedModels.CodeToNodesParser;

public class TypeNodeDefinition
{
    public string Name { get; set; }
    public string FileType { get; set; }
    public List<string> Modifiers { get; set; }

    public List<MethodNodeDefinition> MethodNodes { get; set; }
    public List<FieldNodeDefinition> FieldNodes { get; set; }
    public List<PropertyNodeDefinition> PropertyNodes { get; set; }

    public TypeNodeDefinition()
    {
        Name = "";
        FileType = "";
        MethodNodes = new List<MethodNodeDefinition>();
        FieldNodes = new List<FieldNodeDefinition>();
        PropertyNodes = new List<PropertyNodeDefinition>();
    }
}
