using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.DiagramParsers;

namespace Diagramer.Infrastructure.DiagramParsers.Core;

public interface IUmlParser
{
    void ResolveDependencies(ISettingsHelper settingsHelper);
    Result<string> GetClassDiagram(Request<GetDiagramRequest> getClassRequest);
    Result<string> GetDependencyDiagram(Request<GetDiagramRequest> getClassRequest);
    Result<string> GetIndividualDiagram(Request<TypeNodeDefinition> getClassRequest);
}