namespace Diagramer.Infrastructure.DiagramParsers.PlantUML.Core;

public class TypeSyntaxTemplates
{
    /// <summary>
    /// 0 = Modifiers
    /// 1 = Type
    /// 2 = Name
    /// 3 = Fields
    /// 4 = Properties
    /// 5 = methods
    /// 6 = Open bracket
    /// 7 = Closed bracket
    /// </summary>
    public const string COMPLEX_CLASS = 
        @"{1} {2} {0}
{6}
{3}
{4}
{5}
{7}
        ";
    
    /// <summary>
    /// 0 = Modifiers
    /// 1 = Type
    /// 2 = Name
    /// 3 = Open bracket
    /// 4 = Closed bracket
    /// </summary>
    public const string SIMPLE_CLASS = 
        @"{1} {2} {0}
        {3}
        {4}";

    /// <summary>
    /// 0 = Modifiers
    /// 1 = Name
    /// 2 = Parameters
    /// 3 = DataType
    /// </summary>
    public const string METHOD = "{0} {1}({2}) : {3}";

    /// <summary>
    /// 0 = Modifiers
    /// 1 = Name
    /// 2 = DataType
    /// </summary>
    public const string FIELD = "{0} {1} : {2}";
    
    /// <summary>
    /// 0 = Modifiers
    /// 1 = Name
    /// 2 = DataType
    /// </summary>
    public const string PROPERTY_FULL = "{0} {1} : {2} <<get>> <<set>>";
    
    /// <summary>
    /// 0 = Modifiers
    /// 1 = Name
    /// 2 = DataType
    /// </summary>
    public const string PROPERTY_GETTER = "{0} {1} : {2} <<get>>";

    public const string METHOD_ARGUMENT = "{0}: {1}";

    public const string AGGREGATION = "{0} --o {1}";
    public const string COMPOSITION = "{0} --* {1}";
    public const string INHERITANCE = "{0} --|> {1}";
    public const string IMPLEMENTATION = "{0} ..|> {1}";
    public const string ASSOCIATION = "{0} --> {1}";
}