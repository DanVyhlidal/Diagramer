using Diagramer.Repositories.Core.Serializers;
using Diagramer.SharedModels.Core;

namespace Diagramer.Repositories.Core;

public abstract class ARepository<TData> : IRepository<TData> where TData : class, new()
{
    private readonly Dictionary<int, TData> repository = new();

    private readonly JsonSerializer<TData> dataSerializer;
    private int lastId;

    public ARepository(JsonSerializer<TData> dataSerializer)
    {
        this.dataSerializer = dataSerializer;
        lastId = -1;
        
        LoadData();
    }
    
    public async void LoadData()
    {
        Result<List<TData>> data = await dataSerializer.LoadData();
        if (data.HasError)
        {
            Console.WriteLine(data.ErrorMessage);
            return;
        }

        foreach (TData item in data.ResultObject)
        {
            repository.Add(++lastId, item);
        }
    }
    

    public TData? Get(int id)
    {
        if (repository.ContainsKey(id))
        {
            return repository[id];
        }
        return null;
    }

    public List<(int id, TData data)> GetAll()
    {
        var items = new List<(int id, TData data)>();
        
        foreach (var item in repository)
        {
            items.Add((id: item.Key, data: item.Value));
        }
        
        return items;
    }

    public async Task<Result<bool>> Delete(int id)
    {
        if (repository.ContainsKey(id))
        {
            repository.Remove(id);
        }
        
        return new Result<bool>(await SaveData());
    }

    public async Task<Result<bool>> DeleteAll()
    {
        repository.Clear();
        lastId = -1;
        
        return new Result<bool>(await SaveData());
    }

    public async Task<Result<bool>> Update(TData data, int id)
    {
        if (!repository.ContainsKey(id))
        {
            return new Result<bool>(false);
        }
        repository[id] = data;
        return new Result<bool>(await SaveData());
    }

    public async Task<Result<bool>> UpdateAll(List<TData> data)
    {
        Result<bool> deleteResult = await DeleteAll();
        if (deleteResult.HasError)
        {
            return new Result<bool>(deleteResult.ErrorMessage);
        }

        return await Add(data);
    }

    public async Task<Result<int>> Add(TData data)
    {
        repository.Add(++lastId, data);
        await SaveData();
        return new Result<int>(lastId);
    }

    public async Task<Result<bool>> Add(List<TData> data)
    {
        data.ForEach(x => repository.Add(++lastId, x));
        
        return new Result<bool>(await SaveData());
    }
    
    
    private async Task<bool> SaveData()
    {
        Result<List<TData>> saveDataResult = await dataSerializer.SaveData(repository.Values.ToList());

        if (saveDataResult.HasError)
        {
            Console.WriteLine(saveDataResult.ErrorMessage);
            return false;
        }

        return true;
    }
}