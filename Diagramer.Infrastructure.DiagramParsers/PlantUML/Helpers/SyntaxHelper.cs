using Diagramer.Infrastructure.DiagramParsers.PlantUML.Core;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using SmartFormat;

namespace Diagramer.Infrastructure.DiagramParsers.PlantUML.Helpers;

public static class SyntaxHelper
{
    public static List<string> GetMemberModifiers(List<string> methodModifiers, ISettingsHelper settingsHelper)
    {
        List<string> convertedModifiers = new List<string>();
        
        foreach (string modifier in methodModifiers)
        {
            convertedModifiers.Add(settingsHelper.FindMemberModifier(modifier));
            convertedModifiers.Add(" ");
        }

        if (convertedModifiers.Count > 0)
        {
            convertedModifiers.Remove(convertedModifiers.Last());
        }
        return convertedModifiers;
    }

    public static List<string> GetArguments(Dictionary<string, string> arguments)
    {
        List<string> convertedArguments = new List<string>();
        
        foreach (KeyValuePair<string, string> argument in arguments)
        {
            string convertedArgument = Smart.Format(TypeSyntaxTemplates.METHOD_ARGUMENT, argument.Key, argument.Value);
            
            convertedArguments.Add(convertedArgument);
            
            convertedArguments.Add(", ");
        }

        if (convertedArguments.Count() > 0)
        {
            convertedArguments.RemoveAt(convertedArguments.Count() - 1);
        }
        return convertedArguments;
    }
    
    public static List<string> GetTypeModifiers(List<string> typeModifiers, ISettingsHelper settingsHelper)
    {
        List<string> convertedModifiers = new List<string>();
        
        foreach (string modifier in typeModifiers)
        {
            convertedModifiers.Add(settingsHelper.FindTypeModifier(modifier));
            convertedModifiers.Add(" ");
        }

        if (convertedModifiers.Count > 0)
        {
            convertedModifiers.Remove(convertedModifiers.Last());
        }
        return convertedModifiers;
    }
    
    public static string GetTypeKeyword(string keyword, ISettingsHelper settingsHelper)
    {
        return settingsHelper.FindTypeKeyword(keyword);
    }

    public static string GetDependency(string typeA, string typeB, DependencyType dependencyType)
    {
        string template = string.Empty;

        if (dependencyType == DependencyType.Aggregation)
        {
            template = TypeSyntaxTemplates.AGGREGATION;
        }
        else if (dependencyType == DependencyType.Composition)
        {
            template = TypeSyntaxTemplates.COMPOSITION;
        }
        else if (dependencyType == DependencyType.Inheritance)
        {
            template = TypeSyntaxTemplates.INHERITANCE;
        }
        else if(dependencyType == DependencyType.Implementation)
        {
            template = TypeSyntaxTemplates.IMPLEMENTATION;
        }
        else
        {
            template = TypeSyntaxTemplates.ASSOCIATION;
        }
        
        return Smart.Format(template, typeA, typeB);
    }
}