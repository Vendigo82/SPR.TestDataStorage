using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using static SPR.TestDataStorage.WebService.Controllers.VersionController;

namespace SPR.TestDataStorage.WebService.Tests;

public class DefaultEndpointsTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient httpClient;

    public DefaultEndpointsTests(WebApplicationFactory<Startup> factory)
    {
        httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task VersionEndpoint_ShouldBe200Ok()
    {
        var response = await httpClient.GetAsync("/version");

        response.Should().Be200Ok();
        var payload = await response.Content.ReadFromJsonAsync<VersionModel>();
        payload!.Version.Should().Match("*.*.*.*");
    }
}
