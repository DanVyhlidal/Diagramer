using Diagramer.Infrastructure.CodeParsers.Core;
using Diagramer.Infrastructure.FileManagement;
using Diagramer.Services.CodeParser.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;

namespace Diagramer.Services.CodeParser;

public class CodeParserService<TParser> : ICodeParserService where TParser : ITypeNodeParser, new()
{
    private readonly TParser typeNodeParser;
        
    public CodeParserService()
    {
        typeNodeParser = new TParser();
    }

    public Result<List<TypeNodeDefinition>> GetTypeNodes(Request<List<string>> filesContents)
    {
        Result<List<string>> getFilesContentResult = FileDeserializer.GetFilesContent(filesContents.RequestObject);

        if (getFilesContentResult.HasError)
        {
            return new Result<List<TypeNodeDefinition>>(getFilesContentResult.ErrorMessage);
        }

        Result<List<TypeNodeDefinition>> getTypesNodeResult = typeNodeParser.GetTypeNodes(getFilesContentResult.ResultObject);

        if (getTypesNodeResult.HasError)
        {
            return new Result<List<TypeNodeDefinition>>(getTypesNodeResult.ErrorMessage);
        }

        return new Result<List<TypeNodeDefinition>>(getTypesNodeResult.ResultObject);
    }

    public Result<Dictionary<string, List<DependencyDefinition>>> GetDependencies(Request<List<string>> paths)
    {
        Result<Dictionary<string, List<DependencyDefinition>>> getDependenciesResult = typeNodeParser.GetDependencies(paths.RequestObject);

        if (getDependenciesResult.HasError)
        {
            return new Result<Dictionary<string, List<DependencyDefinition>>>(getDependenciesResult.ErrorMessage);
        }

        return new Result<Dictionary<string, List<DependencyDefinition>>>(getDependenciesResult.ResultObject);
    }
}

