namespace Diagramer.SharedModels.CodeToNodesParser;

public enum DependencyType
{
    Association,
    Inheritance,
    Implementation,
    Aggregation,
    Composition
}

public class DependencyDefinition
{
    public string TypeName { get; set; }
    public DependencyType DependencyType { get; set; }

    public DependencyDefinition()
    {
        TypeName = "";
        DependencyType = DependencyType.Association;
    }
}