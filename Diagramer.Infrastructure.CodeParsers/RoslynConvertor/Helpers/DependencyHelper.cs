using Diagramer.SharedModels.CodeToNodesParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Diagramer.Infrastructure.CodeParsers.RoslynConvertor.Helpers;

public static class DependencyHelper
{
    public static void CheckType(ClassDeclarationSyntax classDeclaration, SemanticModel model, INamedTypeSymbol typeSymbol, Dictionary<string,List<DependencyDefinition>> dependencies)
    {
        if (classDeclaration.BaseList != null)
        {
            foreach (var baseType in classDeclaration.BaseList.Types)
            {
                var baseTypeSymbol = ModelExtensions.GetTypeInfo(model, baseType.Type).Type as INamedTypeSymbol;

                if (baseTypeSymbol == null){ continue; }

                if (baseTypeSymbol.TypeKind == TypeKind.Interface)
                {
                    AddDependency(dependencies,  typeSymbol, baseTypeSymbol, DependencyType.Implementation);
                }
                
                if (baseTypeSymbol.TypeKind == TypeKind.Class)
                {
                    AddDependency(dependencies, typeSymbol, baseTypeSymbol, DependencyType.Inheritance);
                }
                
            }
        }
    }

    public static void CheckMembers(TypeDeclarationSyntax typeDeclaration, SemanticModel model, INamedTypeSymbol typeSymbol, Dictionary<string,List<DependencyDefinition>> dependencies)
    {
        foreach (FieldDeclarationSyntax field in typeDeclaration.Members.OfType<FieldDeclarationSyntax>())
        {
            ITypeSymbol fieldTypeSymbol = model.GetTypeInfo(field.Declaration.Type).Type;
            
            if (fieldTypeSymbol == null && fieldTypeSymbol.SpecialType != SpecialType.None) { continue; }
            
            INamedTypeSymbol correctSymbol = fieldTypeSymbol.GetCorrectSymbol();
                
            if(correctSymbol == null) { continue; }
            
            AddDependency(dependencies, typeSymbol, correctSymbol, correctSymbol.GetDependencyAssociation());
        }
        
        foreach (PropertyDeclarationSyntax property in typeDeclaration.Members.OfType<PropertyDeclarationSyntax>())
        {
            ITypeSymbol propertyTypeSymbol = model.GetTypeInfo(property.Type).Type;

            if (propertyTypeSymbol == null && propertyTypeSymbol.SpecialType != SpecialType.None) { continue; }
            
            INamedTypeSymbol correctSymbol = propertyTypeSymbol.GetCorrectSymbol();
                
            if(correctSymbol == null) { continue; }
            
            AddDependency(dependencies, typeSymbol, correctSymbol, correctSymbol.GetDependencyAssociation());
        }
    }

    public static void CheckMethods(TypeDeclarationSyntax typeDeclaration, SemanticModel model, INamedTypeSymbol typeSymbol, Dictionary<string,List<DependencyDefinition>> dependencies)
    {
        foreach (MethodDeclarationSyntax method in typeDeclaration.Members.OfType<MethodDeclarationSyntax>())
        {
            foreach (ParameterSyntax argument in method.ParameterList.Parameters)
            {
                if (argument.Type == null) { continue; }
                
                ITypeSymbol argumentTypeSymbol = model.GetTypeInfo(argument.Type).Type;
                
                if (argumentTypeSymbol == null && argumentTypeSymbol.SpecialType != SpecialType.None) { continue; }

                INamedTypeSymbol correctSymbol = argumentTypeSymbol.GetCorrectSymbol();
                
                if(correctSymbol == null) { continue;}
                
                AddDependency(dependencies, typeSymbol,correctSymbol , correctSymbol.GetDependencyAssociation());
            }

            if (method.Body == null) { continue; }
            
            IEnumerable<VariableDeclarationSyntax> variableDeclarationNodes = method.Body.DescendantNodesAndSelf().OfType<VariableDeclarationSyntax>();
                            
            foreach (VariableDeclarationSyntax variableDeclarationNode in variableDeclarationNodes)
            {
                ITypeSymbol variableDeclarationTypeSymbol = model.GetTypeInfo(variableDeclarationNode.Type).Type;
                
                if (variableDeclarationTypeSymbol == null && variableDeclarationTypeSymbol.SpecialType != SpecialType.None) { continue; }
                
                INamedTypeSymbol correctSymbol = variableDeclarationTypeSymbol.GetCorrectSymbol();
                
                if(correctSymbol == null) { continue;}
                
                AddDependency(dependencies, typeSymbol, correctSymbol, correctSymbol.GetDependencyAssociation());
            }
        }
    }


    private static INamedTypeSymbol GetCorrectSymbol(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol genericTypeSymbol && genericTypeSymbol.IsGenericType)
        {
            IEnumerable<ITypeSymbol> symbolArguments = genericTypeSymbol.TypeArguments.Where(x => x.SpecialType == SpecialType.None);

            if (symbolArguments == null || symbolArguments.Count() == 0)
            {
                return null;
            }

            foreach (ITypeSymbol symbolArgument in symbolArguments)
            {
                if (symbolArgument is INamedTypeSymbol genericSymbol)
                {
                    return GetCorrectSymbol(symbolArgument);
                }
            }
            
            return symbolArguments.First() as INamedTypeSymbol;
        }
        
        if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
        {
            return arrayTypeSymbol.ElementType as INamedTypeSymbol;
        }

        if (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.SpecialType != SpecialType.None)
        {
            return null;
        }
        
        return typeSymbol as INamedTypeSymbol;
    }

    private static DependencyType GetDependencyAssociation(this INamedTypeSymbol symbol)
    {
        if (symbol.IsReferenceType && !symbol.IsValueType)
        {
            return DependencyType.Aggregation;
        }

        if (symbol.IsReferenceType && symbol.IsValueType)
        {
            return DependencyType.Composition;
        }
        
        return DependencyType.Association;
    }
    
    private static void AddDependency(Dictionary<string, List<DependencyDefinition>> dependencies,
        INamedTypeSymbol sourceType, INamedTypeSymbol targetType, DependencyType dependencyType)
    {
        if (targetType.Name == string.Empty)
        {
            return;
        }
        
        var dependencyDefinition = new DependencyDefinition
        {
            TypeName = targetType.Name,
            DependencyType = dependencyType
        };
        
        if (dependencies.ContainsKey(sourceType.Name))
        {
            if (dependencies[sourceType.Name].Any(dd => dd.TypeName == dependencyDefinition.TypeName))
            {
                return;
            }
            dependencies[sourceType.Name].Add(dependencyDefinition);
            return;
        }
        
        dependencies.Add(sourceType.Name, new List<DependencyDefinition>{dependencyDefinition});
    }
}