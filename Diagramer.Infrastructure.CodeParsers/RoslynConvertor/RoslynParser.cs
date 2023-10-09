using Diagramer.Infrastructure.CodeParsers.Core;
using Diagramer.Infrastructure.CodeParsers.RoslynConvertor.Helpers;
using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Diagramer.Infrastructure.CodeParsers.RoslynConvertor;

public class RoslynParser : ITypeNodeParser
{
    private RoslynHelper roslynHelper;

    public RoslynParser()
    {
        roslynHelper = new RoslynHelper();
    }

    public Result<List<TypeNodeDefinition>> GetTypeNodes(List<string> fileContents)
    {
        Result<List<TypeDeclarationSyntax>> getTypeSyntaxesResult =
            roslynHelper.GetTypeDeclarationSyntaxes(fileContents);

        if (getTypeSyntaxesResult.HasError)
        {
            return new Result<List<TypeNodeDefinition>>(getTypeSyntaxesResult.ErrorMessage);
        }

        List<TypeNodeDefinition> typeNodeDefinitions =
            roslynHelper.GetTypeNodeDefinitions(getTypeSyntaxesResult.ResultObject);

        return new Result<List<TypeNodeDefinition>>(typeNodeDefinitions);
    }

    public Result<Dictionary<string, List<DependencyDefinition>>> GetDependencies(List<string> filePaths)
    {
        Dictionary<string, List<DependencyDefinition>> dependencies = new Dictionary<string, List<DependencyDefinition>>();

        CSharpCompilation compilation = roslynHelper.GetCompilationUnit(filePaths);

        foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(syntaxTree);
            
            IEnumerable<TypeDeclarationSyntax> typeDeclarations =
                syntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>();

            foreach (TypeDeclarationSyntax typeDeclaration in typeDeclarations)
            {
                INamedTypeSymbol typeSymbol = model.GetDeclaredSymbol(typeDeclaration);

                if (typeDeclaration is not ClassDeclarationSyntax classDeclaration) { continue; }
                
                DependencyHelper.CheckType(classDeclaration, model, typeSymbol, dependencies);
                DependencyHelper.CheckMethods(typeDeclaration, model, typeSymbol, dependencies);
                DependencyHelper.CheckMembers(typeDeclaration, model, typeSymbol, dependencies);
            }
        }

        return new Result<Dictionary<string, List<DependencyDefinition>>>(dependencies);
    }
    
}