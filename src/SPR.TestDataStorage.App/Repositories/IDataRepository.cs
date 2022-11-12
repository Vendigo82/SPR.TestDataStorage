using SPR.TestDataStorage.App.Models;

namespace SPR.TestDataStorage.App.Repositories;

public interface IDataRepository
{
    public Task<bool> IsProjectExistsAsync(string project);

    public Task<bool> IsObjectTypeExistsAsync(string objectType);

    public Task SaveDataAsync(DataContent data);
}
