using Microsoft.EntityFrameworkCore;
using SPR.TestDataStorage.App.Models;
using SPR.TestDataStorage.App.Repositories;
using SPR.TestDataStorage.Infra.Data;
using SPR.TestDataStorage.Infra.Models;

namespace SPR.TestDataStorage.Infra.Repositories;

public class DataRepository : IDataRepository
{
    private readonly SPRTestDataStorageContext context;

    public DataRepository(SPRTestDataStorageContext context)
    {
        this.context = context;
    }

    public Task<bool> IsObjectTypeExistsAsync(string objectType)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsProjectExistsAsync(string project)
    {
        return Task.FromResult(true);
    }

    public async Task SaveDataAsync(DataContent data)
    {
        var dbHeader = new DataHeaderModel
        {
            Id = Guid.NewGuid(),
            ProjectId = context.Projects.AsNoTracking().First().Id,
            ObjectTypeId = context.ObjectTypes.AsNoTracking().First().Id,
            ObjectIdentity = data.ObjectIdentity,
            DataName = "111"
        };
        await context.DataHeaders.AddAsync(dbHeader);
        await context.SaveChangesAsync();
    }
}
