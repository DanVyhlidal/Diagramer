using Diagramer.Infrastructure.CodeParsers.RoslynConvertor.Extensions;
using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Diagramer.Infrastructure.CodeParsers.RoslynConvertor.Helpers;

public class RoslynHelper
{
    public Result<List<TypeDeclarationSyntax>> GetTypeDeclarationSyntaxes(List<string> fileContents)
    {
        List<TypeDeclarationSyntax> typeDeclarations = new List<TypeDeclarationSyntax>();
        
        foreach (string content in fileContents)
        {
            CompilationUnitSyntax root = GetRoot(content);
            
            IEnumerable<TypeDeclarationSyntax> typeDeclarationsFromFile = root.DescendantNodes().OfType<TypeDeclarationSyntax>();

            if (!typeDeclarationsFromFile!.Any())
            {
                return new Result<List<TypeDeclarationSyntax>>($"File with index [{fileContents.IndexOf(content)}] cannot be converted to TypeDeclarationSyntax");
            }
            
            typeDeclarations.AddRange(typeDeclarationsFromFile);
        }

        return new Result<List<TypeDeclarationSyntax>>(typeDeclarations);
    }
    public List<TypeNodeDefinition> GetTypeNodeDefinitions(List<TypeDeclarationSyntax> typeDeclarationSyntaxes)
    {
        var nodes = new List<TypeNodeDefinition>();
        
        foreach(TypeDeclarationSyntax typeDeclaration in typeDeclarationSyntaxes)
        {
            TypeNodeDefinition typeNode = new TypeNodeDefinition()
            {
                Name = typeDeclaration.GetNameOfTypeNode(),
                FileType = typeDeclaration.GetTypeOfTypeNode(),
                Modifiers = typeDeclaration.GetModefiersOfTypeNode(),

                MethodNodes = typeDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().GetMethodNodes(),
                FieldNodes = typeDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().GetFieldNodes(),
                PropertyNodes = typeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>().GetPropertyNodes()
            };
            nodes.Add(typeNode);
        }
        return nodes;
    }
    
    public CSharpCompilation GetCompilationUnit(List<string> filePaths)
    {
        return CSharpCompilation.Create(
            "MyCompilation",
            syntaxTrees: filePaths.Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path))),
            references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) }
        );
    }
    private CompilationUnitSyntax GetRoot(string data)
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(data);
            
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        return root;
    }
    
}