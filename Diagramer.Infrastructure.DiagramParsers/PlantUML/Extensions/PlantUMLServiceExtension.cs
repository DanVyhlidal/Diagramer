using Diagramer.Infrastructure.DiagramParsers.PlantUML.Core;
using Diagramer.Infrastructure.DiagramParsers.PlantUML.Helpers;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using SmartFormat;

namespace Diagramer.Infrastructure.DiagramParsers.PlantUML.Extensions;

public static class PlantUmlServiceExtension
{
    public static string ConvertMethods(this List<MethodNodeDefinition> methodNodeDefintions, ISettingsHelper settingsHelper)
    {
        if (methodNodeDefintions.Count == 0) { return String.Empty; }
        
        List<string> convertedMethods = new List<string>();

        foreach (MethodNodeDefinition methodNode in methodNodeDefintions)
        {
            List<string> modifiers = SyntaxHelper.GetMemberModifiers(methodNode.Modifiers, settingsHelper);
            List<string> arguments = SyntaxHelper.GetArguments(methodNode.Arguments);
            
            string convertedMethod = Smart.Format(TypeSyntaxTemplates.METHOD, string.Join("",modifiers),  methodNode.Name, string.Join("",arguments) ,methodNode.DataType);
            
            convertedMethods.Add(convertedMethod);
        }
        
        return string.Join("\n", convertedMethods);
    }
    public static string ConvertFields(this List<FieldNodeDefinition> fieldNodeDefinitions, ISettingsHelper settingsHelper)
    {
        if (fieldNodeDefinitions.Count == 0) { return String.Empty; }
        
        List<string> convertedFields = new List<string>();

        foreach (FieldNodeDefinition fieldNode in fieldNodeDefinitions)
        {
            List<string> modifiers = SyntaxHelper.GetMemberModifiers(fieldNode.Modifiers, settingsHelper);
            
            string convertedField = Smart.Format(TypeSyntaxTemplates.FIELD, string.Join("", modifiers),  fieldNode.Name ,fieldNode.DataType);
            
            convertedFields.Add(convertedField);
        }
        
        return string.Join("\n", convertedFields);
    }
    public static string ConvertProperties(this List<PropertyNodeDefinition> propertyNodeDefinitions, ISettingsHelper settingsHelper)
    {
        if (propertyNodeDefinitions.Count == 0) { return String.Empty; }
        
        List<string> convertedProperties = new List<string>();

        foreach (PropertyNodeDefinition propertyNode in propertyNodeDefinitions)
        {
            List<string> modifiers = SyntaxHelper.GetMemberModifiers(propertyNode.Modifiers, settingsHelper);

            string syntaxTemplate = propertyNode.IsGetterOnly ? TypeSyntaxTemplates.PROPERTY_GETTER : TypeSyntaxTemplates.PROPERTY_FULL;
            
            string convertedProperty = Smart.Format(syntaxTemplate, string.Join("", modifiers),  propertyNode.Name ,propertyNode.DataType);
          
            convertedProperties.Add(convertedProperty);
        }
        
        return string.Join("\n", convertedProperties);
    }

    public static string WrapDiagram(this string diagram) => string.Join("\n","@startuml", diagram, "@enduml");
}