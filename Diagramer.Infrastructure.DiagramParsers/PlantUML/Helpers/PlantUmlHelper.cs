using Diagramer.Infrastructure.DiagramParsers.PlantUML.Core;
using Diagramer.Infrastructure.DiagramParsers.PlantUML.Extensions;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using SmartFormat;

namespace Diagramer.Infrastructure.DiagramParsers.PlantUML.Helpers;

public static class PlantUmlHelper
{
    public static string GetComplexClass(this TypeNodeDefinition nodeDefinition, ISettingsHelper settingsHelper)
    {
        string typeKeyword = SyntaxHelper.GetTypeKeyword(nodeDefinition.FileType, settingsHelper);
        List<string> typeModifiers = SyntaxHelper.GetTypeModifiers(nodeDefinition.Modifiers, settingsHelper);

        string fields = nodeDefinition.FieldNodes.ConvertFields(settingsHelper);
        string properties = nodeDefinition.PropertyNodes.ConvertProperties(settingsHelper);
        string methods = nodeDefinition.MethodNodes.ConvertMethods(settingsHelper);
        
        string fullClass = Smart.Format(
            TypeSyntaxTemplates.COMPLEX_CLASS, 
            string.Join("", typeModifiers),
            typeKeyword,
            nodeDefinition.Name,
            fields,
            properties,
            methods,
            "{",
            "}");
        
        return fullClass;
    }
    
    public static string GetSimpleClass(this TypeNodeDefinition nodeDefinition, ISettingsHelper settingsHelper)
    {
        string typeKeyword = SyntaxHelper.GetTypeKeyword(nodeDefinition.FileType, settingsHelper);
        List<string> typeModifiers = SyntaxHelper.GetTypeModifiers(nodeDefinition.Modifiers, settingsHelper);

        string fullClass =  Smart.Format(
            TypeSyntaxTemplates.SIMPLE_CLASS,
            string.Join("", typeModifiers),
            typeKeyword,
            nodeDefinition.Name,
            "{",
            "}");

        return fullClass;
    }

    public static List<string> GetDependencies(this Dictionary<string, List<DependencyDefinition>> dependencies)
    {
        List<string> textDependencies = new List<string>();

        foreach (string dependency in dependencies.Keys)
        {
            dependencies.TryGetValue(dependency, out List<DependencyDefinition> dependencyList);

            foreach (DependencyDefinition dependencyDefinition in dependencyList)
            {
                textDependencies.Add(SyntaxHelper.GetDependency(dependency, dependencyDefinition.TypeName, dependencyDefinition.DependencyType));
            }
        }

        return textDependencies;
    }
    
}