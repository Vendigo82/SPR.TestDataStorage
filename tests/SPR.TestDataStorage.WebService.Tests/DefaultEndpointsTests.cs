using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        JObject payload = JObject.Parse(await response.Content.ReadAsStringAsync());
        payload["version"]!.Value<string>().Should().Match("*.*.*.*");
    }

    [Theory]
    [InlineData("/health")]
    public async Task HealthEndpoint_ShouldBe200Ok(string url)
    {
        var response = await httpClient.GetAsync(url);

        response.Should().Be200Ok();
    }
}
