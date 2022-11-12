using SPR.TestDataStorage.App.Models;
using SPR.TestDataStorage.App.Repositories;

namespace SPR.TestDataStorage.App.UseCases.SaveData;

public enum SaveDataResult
{
    Success,
    ProjectNotFound,
    ObjectTypeNotFound
}

public interface ISaveDataUseCase
{
    Task<SaveDataResult> SaveDataAsync(DataContent data);
}

public class SaveDataUseCase : ISaveDataUseCase
{
    private readonly IDataRepository repository;

    public SaveDataUseCase(IDataRepository repository)
    {
        this.repository = repository;
    }

    public async Task<SaveDataResult> SaveDataAsync(DataContent data)
    {
        if (!await repository.IsProjectExistsAsync(data.Project))
            return SaveDataResult.ProjectNotFound;

        if (!await repository.IsObjectTypeExistsAsync(data.ObjectType))
            return SaveDataResult.ObjectTypeNotFound;

        // TODO: also verify data sections for given object type

        await repository.SaveDataAsync(data);
        return SaveDataResult.Success;
    }
}
