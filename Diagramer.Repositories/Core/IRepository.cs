using Diagramer.SharedModels.Core;

namespace Diagramer.Repositories.Core;

public interface IRepository<TData> where TData : class
{
    TData? Get(int id);
    List<(int id, TData data)> GetAll();
    Task<Result<bool>> Delete(int id);
    Task<Result<bool>> DeleteAll();
    Task<Result<bool>> Update(TData data, int id);
    Task<Result<bool>> UpdateAll(List<TData> data);
    Task<Result<int>> Add(TData data);
    Task<Result<bool>> Add(List<TData> data);
}