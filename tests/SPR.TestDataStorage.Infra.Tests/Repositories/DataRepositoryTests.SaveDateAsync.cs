using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SPR.TestDataStorage.App.Models;

namespace SPR.TestDataStorage.Infra.Repositories;

public partial class DataRepositoryTests
{
    [Fact]
    public async Task SaveDataAsync_Test()
    {
        // setup
        var model = new DataContent()
        {
            ObjectIdentity = Guid.NewGuid().ToString()
        };

        // action
        await target.SaveDataAsync(model);

        // asserts
        var dbItem = await context.DataHeaders
            .FirstOrDefaultAsync(i => i.ObjectIdentity == model.ObjectIdentity);
        dbItem.Should().NotBeNull();
    }
}
