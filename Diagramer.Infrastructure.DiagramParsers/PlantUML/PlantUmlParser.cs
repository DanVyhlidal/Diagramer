using Diagramer.Infrastructure.DiagramParsers.Core;
using Diagramer.Infrastructure.DiagramParsers.PlantUML.Extensions;
using Diagramer.Infrastructure.DiagramParsers.PlantUML.Helpers;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.DiagramParsers;

namespace Diagramer.Infrastructure.DiagramParsers.PlantUML;

public class PlantUmlParser : IUmlParser
{
    private ISettingsHelper settingsHelper;
    
    public void ResolveDependencies(ISettingsHelper settingsHelper)
    {
        this.settingsHelper = settingsHelper;
        
    }
    
    public Result<string> GetClassDiagram(Request<GetDiagramRequest> getClassRequest)
    {
        List<TypeNodeDefinition> typeNodes = getClassRequest.RequestObject.TypeNodes;
        Dictionary<string, List<DependencyDefinition>> dependencies = getClassRequest.RequestObject.Dependencies;

        List<string> classDiagram = new List<string>();
        foreach (TypeNodeDefinition typeNodeDefinition in typeNodes)
        {
            classDiagram.Add(typeNodeDefinition.GetComplexClass(settingsHelper));
        }
        
        classDiagram.AddRange(dependencies.GetDependencies());

        return new Result<string>(resultObject: string.Join("\n", classDiagram).WrapDiagram());
    }

    public Result<string> GetDependencyDiagram(Request<GetDiagramRequest> getClassRequest)
    {
        List<TypeNodeDefinition> typeNodes = getClassRequest.RequestObject.TypeNodes;
        Dictionary<string, List<DependencyDefinition>> dependencies = getClassRequest.RequestObject.Dependencies;

        List<string> classDiagram = new List<string>();
        foreach (TypeNodeDefinition typeNodeDefinition in typeNodes)
        {
            classDiagram.Add(typeNodeDefinition.GetSimpleClass(settingsHelper));
        }
        
        classDiagram.AddRange(dependencies.GetDependencies());

        return new Result<string>(resultObject: string.Join("\n", classDiagram).WrapDiagram());
    }

    public Result<string> GetIndividualDiagram(Request<TypeNodeDefinition> getClassRequest)
    {
        TypeNodeDefinition nodeDefinition = getClassRequest.RequestObject;
        
        return new Result<string>(resultObject: nodeDefinition.GetComplexClass(settingsHelper).WrapDiagram());
    }
}