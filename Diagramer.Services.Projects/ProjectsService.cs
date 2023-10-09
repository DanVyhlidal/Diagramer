using Diagramer.Infrastructure.FileManagement;
using Diagramer.Repositories.Projects.Interfaces;
using Diagramer.Services.Projects.Core;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Project;

namespace Diagramer.Services.Projects;

public class ProjectsService : IProjectsService
{
    private readonly IProjectsRepository projectsRepository;

    public ProjectsService(IProjectsRepository projectsRepository)
    {
        this.projectsRepository = projectsRepository;
    }

    public async Task<Result<int>> CreateNewProject(ProjectDefinition projectDefinition)
    {
        List<string> paths = new List<string>();

        int csProjCounter = 0;
        foreach (string filePath in projectDefinition.FilePaths)
        {
            if (filePath.EndsWith(ProjectsConstants.CSPROJ_FILE))
            {
                List<string> projectFiles = FileDeserializer.GetFilesPathsFromCsProj(filePath);

                if (projectFiles.Count == 0)
                {
                    return new Result<int>(
                        $"There are no found file paths for {projectDefinition.FilePaths.FirstOrDefault()}");
                }
                paths.AddRange(projectFiles);
                csProjCounter++;
                continue;
            }
                
            paths.Add(filePath);
        }

        projectDefinition.Type = csProjCounter > 0
            ? ProjectsConstants.CSPROJ_FILE
            : ProjectsConstants.CS_FILES;
            
        projectDefinition.FilePaths = paths;

        return  await projectsRepository.Add(projectDefinition);
    }

    public Result<List<(int, ProjectDefinition)>> GetAllProjects() =>
        new(projectsRepository.GetAll());

    public Result<ProjectDefinition> GetProject(int id) =>
        new(projectsRepository.Get(id));
}