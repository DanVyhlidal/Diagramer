using System.Text.Json;
using Diagramer.SharedModels.Core;

namespace Diagramer.Repositories.Core.Serializers;

public class JsonSerializer<TData> where TData : class, new()
{
    private readonly string fileName;

    private readonly List<TData>? baseData;

    private static string PersistanceFolder
    {
        get => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiagramerData");
    }

    private string FilePath
    {
        get => Path.Join(PersistanceFolder, $"{fileName}.json");
    }

    public JsonSerializer(string fileName, List<TData>? baseData = null)
    {
        this.fileName = fileName;
        this.baseData = baseData;
    }

    public Task<Result<List<TData>>> LoadData()
    {
        if (!CheckFileExistence(FilePath))
        {
            return baseData != null
                ? SaveData(baseData)
                : Task.FromResult(new Result<List<TData>>($"File '{FilePath}' has not been found!"));
        }

        string jsonString = File.ReadAllText(FilePath);

        List<TData> data = JsonSerializer.Deserialize<List<TData>>(jsonString);

        if (data == null)
            return Task.FromResult(new Result<List<TData>>("No data has been found!"));

        return Task.FromResult(new Result<List<TData>>(data));
    }

    public Task<Result<List<TData>>> SaveData(List<TData> data)
    {
        CheckPersistanceFolder();

        string jsonString = JsonSerializer.Serialize(data);

        File.WriteAllText(FilePath, jsonString);

        return Task.FromResult(new Result<List<TData>>(data));
    }

    private static bool CheckFileExistence(string filePath)
    {
        return File.Exists(filePath);
    }

    private static void CheckPersistanceFolder()
    {
        if (!Directory.Exists(PersistanceFolder))
        {
            Directory.CreateDirectory(PersistanceFolder);
        }
    }
}