using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.TestDataStorage.WebService.Tests;

public class SaveDataTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient httpClient;

    public SaveDataTests(WebApplicationFactory<Startup> factory)
    {
        httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Test()
    {
        // setup
        var objectId = Guid.NewGuid();

        // action
        //var response = await httpClient.PutAsJsonAsync($"/api/data", new
        var response = await httpClient.PutAsJsonAsync($"/api/data/test/test_object/{objectId}/test_name", new
        {
            data1 = new object(),
            data2 = new object(),
        });

        // asserts
        response.Should().Be200Ok();
    }
}
