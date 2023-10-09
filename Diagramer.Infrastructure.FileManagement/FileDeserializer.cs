using Buildalyzer;
using Diagramer.SharedModels.Core;

namespace Diagramer.Infrastructure.FileManagement;

public static class FileDeserializer
{
    public static Result<List<string>> GetFilesContent(List<string> paths)
    {
        List<string> fileContents = new List<string>();
            
        foreach (string path in paths)
        {
            string data = File.ReadAllText(path);

            if (data.Length < 1)
            {
                return new Result<List<string>>($"File with path [{path}] has no code in it.");
            }

            fileContents.Add(data);
        }

        return new Result<List<string>>(fileContents);
    }
    
    public static List<string> GetFilesPathsFromCsProj(string path)
    {
        AnalyzerManager manager = new AnalyzerManager();
        IProjectAnalyzer analyzer = manager.GetProject(path);
        IAnalyzerResults results = analyzer.Build();
        AnalyzerResult result = (AnalyzerResult)results.Single();

        string[] sourceFiles = result.SourceFiles;

        List<string> paths = new List<string>();
        
        foreach (string file in sourceFiles)
        {
            string[] fileSplitter = file.Split('\\');
            int freq = fileSplitter.Last().Count(x => (x == '.'));
            if (freq > 1 || fileSplitter.Last().Contains("AssemblyInfo")) { continue; }
            
            paths.Add(file);
        }

        return paths;
    }
}

