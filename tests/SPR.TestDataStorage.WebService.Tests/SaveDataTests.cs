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
using Xunit.Abstractions;

namespace SPR.TestDataStorage.WebService.Tests;

public class SaveDataTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient httpClient;
    private readonly WebApplicationFactory<Startup> factory;
    private readonly ITestOutputHelper outputHelper;

    public SaveDataTests(WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
    {
        httpClient = factory.CreateClient();
        this.factory = factory;
        this.outputHelper = outputHelper;
    }

    [Fact]
    public async Task Test()
    {
        // setup
        var objectId = Guid.NewGuid().ToString();

        // action
        var response = await httpClient.PutAsJsonAsync($"/api/data/test/test_object/{objectId}/test_name", new
        {
            data1 = new object(),
            data2 = new object(),
        });

        // asserts
        await outputHelper.WriteContentBodyAsync(response);

        response.Should().Be200Ok();

        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SPRTestDataStorageContext>();

        var dbItem = await context.DataHeaders.FirstOrDefaultAsync(i => i.ObjectIdentity == objectId);
        dbItem.Should().NotBeNull();
    }
}
