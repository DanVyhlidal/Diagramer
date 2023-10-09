using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;

namespace Diagramer.Services.CodeParser.Core;

public interface ICodeParserService
{
    Result<List<TypeNodeDefinition>> GetTypeNodes(Request<List<string>> filesContents);
    Result<Dictionary<string, List<DependencyDefinition>>> GetDependencies(Request<List<string>> paths);
}
