using Diagramer.SharedModels.CodeToNodesParser;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Diagramer.Infrastructure.CodeParsers.RoslynConvertor.Extensions;

internal static class TypeNodeServiceExtension
{
    public static string GetNameOfTypeNode(this TypeDeclarationSyntax type) => type.Identifier.ValueText;
    public static string GetTypeOfTypeNode(this TypeDeclarationSyntax type) => type.Keyword.ValueText;

    public static List<string> GetModefiersOfTypeNode(this TypeDeclarationSyntax type) =>
        type.Modifiers.Select(x => x.ValueText).ToList();

    public static List<MethodNodeDefinition> GetMethodNodes(this IEnumerable<MethodDeclarationSyntax> methods)
    {
        var methodNodes = new List<MethodNodeDefinition>();

        foreach (var method in methods)
        {
            MethodNodeDefinition node = new MethodNodeDefinition();
            node.Name = method.Identifier.ValueText;
            node.DataType = method.ReturnType.ToString();

            foreach (var parameter in method.ParameterList.Parameters)
            {
                string parameterName = parameter.Identifier.ValueText;
                string parameterType = parameter.Type!.ToString();
                node.Arguments.Add(parameterName, parameterType);
            }

            foreach (var modifier in method.Modifiers)
            {
                node.Modifiers.Add(modifier.ValueText);
            }

            methodNodes.Add(node);
        }

        return methodNodes;
    }

    public static List<FieldNodeDefinition> GetFieldNodes(this IEnumerable<FieldDeclarationSyntax> fields)
    {
        var fieldNodes = new List<FieldNodeDefinition>();

        foreach (var field in fields)
        {
            FieldNodeDefinition node = new FieldNodeDefinition();

            node.Name = field.Declaration.Variables[0].Identifier.ValueText;
            node.DataType = field.Declaration.Type.ToString();

            foreach (var modifier in field.Modifiers)
            {
                node.Modifiers.Add(modifier.ValueText);
            }

            fieldNodes.Add(node);
        }

        return fieldNodes;
    }

    public static List<PropertyNodeDefinition> GetPropertyNodes(this IEnumerable<PropertyDeclarationSyntax> properties)
    {
        var propertyNodes = new List<PropertyNodeDefinition>();

        foreach (var property in properties)
        {
            PropertyNodeDefinition node = new PropertyNodeDefinition();
            node.Name = property.Identifier.ValueText;
            node.DataType = property.Type.ToString();
            node.IsGetterOnly = property.AccessorList?.Accessors.Count <= 1 || property.AccessorList == null;

            foreach (var modifier in property.Modifiers)
            {
                node.Modifiers.Add(modifier.ValueText);
            }

            propertyNodes.Add(node);
        }

        return propertyNodes;
    }
}