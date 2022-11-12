using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SPR.TestDataStorage.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.TestDataStorage.WebService.Tests;

public class SaveDataTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient httpClient;
    private readonly WebApplicationFactory<Startup> factory;

    public SaveDataTests(WebApplicationFactory<Startup> factory)
    {
        httpClient = factory.CreateClient();
        this.factory = factory;
    }

    [Fact]
    public async Task Test()
    {
        // setup
        var objectId = Guid.NewGuid();

        // action
        var response = await httpClient.PutAsJsonAsync($"/api/data/test/test_object/{objectId}/test_name", new
        {
            data1 = new object(),
            data2 = new object(),
        });

        // asserts
        response.Should().Be200Ok();

        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SPRTestDataStorageContext>();

        var dbItem = await context.DataContents.FirstOrDefaultAsync(i => i.Id == objectId);
        dbItem.Should().NotBeNull();
    }
}
