using Diagramer.SharedModels.Core;

namespace Diagramer.Services.Diagrams.Core;

public interface IDiagramService
{
    Result<string> GenerateClassDiagram(List<string> paths);
    
    Result<string> GenerateDependencyDiagram(List<string> paths);
    Result<string> GenerateIndividualDiagram(string path);
}