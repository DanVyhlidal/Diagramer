using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;

namespace Diagramer.Infrastructure.CodeParsers.Core;

public interface ITypeNodeParser
{
    Result<List<TypeNodeDefinition>> GetTypeNodes(List<string> fileContents);
    Result<Dictionary<string, List<DependencyDefinition>>> GetDependencies(List<string> filePaths);
}