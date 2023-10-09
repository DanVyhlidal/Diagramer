using Diagramer.Infrastructure.DiagramParsers.Core;
using Diagramer.Services.CodeParser.Core;
using Diagramer.Services.Diagrams.Core;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.CodeToNodesParser;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.DiagramParsers;

namespace Diagramer.Services.Diagrams;

public class DiagramService<TUmlParser> : IDiagramService where TUmlParser : IUmlParser, new() {
    
    private readonly ICodeParserService codeParserService;
    private readonly TUmlParser umlParser;

    public DiagramService(ICodeParserService codeParserService, ISettingsHelper settingsHelper)
    {
        this.codeParserService = codeParserService;
        
        umlParser = new TUmlParser();
        umlParser.ResolveDependencies(settingsHelper);
    }
    
    public Result<string> GenerateClassDiagram(List<string> paths)
    {
        Result<List<TypeNodeDefinition>> getTypeNodesResult = codeParserService.GetTypeNodes(new Request<List<string>>
        {
            RequestObject = paths
        });

        if (getTypeNodesResult.HasError)
        {
            return new Result<string>(errorMessage: getTypeNodesResult.ErrorMessage);
        }

        Result<Dictionary<string, List<DependencyDefinition>>> getDependenciesResult = codeParserService.GetDependencies(new Request<List<string>>
        {
            RequestObject = paths
        });
        
        if (getDependenciesResult.HasError)
        {
            return new Result<string>(errorMessage: getDependenciesResult.ErrorMessage);
        }

        Result<string> getClassDiagramResult = umlParser.GetClassDiagram(new Request<GetDiagramRequest>
        {
            RequestObject = new GetDiagramRequest()
            {
                TypeNodes = getTypeNodesResult.ResultObject,
                Dependencies = getDependenciesResult.ResultObject
            }
        });

        if (getClassDiagramResult.HasError)
        {
            return new Result<string>(errorMessage: getClassDiagramResult.ErrorMessage);
        }

        return new Result<string>(resultObject: getClassDiagramResult.ResultObject);
    }

    public Result<string> GenerateDependencyDiagram(List<string> paths)
    {
        Result<List<TypeNodeDefinition>> getTypeNodesResult = codeParserService.GetTypeNodes(new Request<List<string>>
        {
            RequestObject = paths
        });

        if (getTypeNodesResult.HasError)
        {
            return new Result<string>(errorMessage: getTypeNodesResult.ErrorMessage);
        }

        Result<Dictionary<string, List<DependencyDefinition>>> getDependenciesResult = codeParserService.GetDependencies(new Request<List<string>>
        {
            RequestObject = paths
        });
        
        if (getDependenciesResult.HasError)
        {
            return new Result<string>(errorMessage: getDependenciesResult.ErrorMessage);
        }

        Result<string> getDependencyDiagramResult = umlParser.GetDependencyDiagram(new Request<GetDiagramRequest>
        {
            RequestObject = new GetDiagramRequest()
            {
                TypeNodes = getTypeNodesResult.ResultObject,
                Dependencies = getDependenciesResult.ResultObject
            }
        });

        if (getDependencyDiagramResult.HasError)
        {
            return new Result<string>(errorMessage: getDependencyDiagramResult.ErrorMessage);
        }

        return new Result<string>(resultObject: getDependencyDiagramResult.ResultObject);
    }


    public Result<string> GenerateIndividualDiagram(string path)
    {
        Result<List<TypeNodeDefinition>> getTypeNodesResult = codeParserService.GetTypeNodes(new Request<List<string>>
        {
            RequestObject = new List<string>{path}
        });

        if (getTypeNodesResult.HasError)
        {
            return new Result<string>(errorMessage: getTypeNodesResult.ErrorMessage);
        }

        Result<string> getDependencyDiagramResult = umlParser.GetIndividualDiagram(new Request<TypeNodeDefinition>
        {
            RequestObject = getTypeNodesResult.ResultObject.FirstOrDefault()
        });

        if (getDependencyDiagramResult.HasError)
        {
            return new Result<string>(errorMessage: getDependencyDiagramResult.ErrorMessage);
        }

        return new Result<string>(resultObject: getDependencyDiagramResult.ResultObject);
    }
}