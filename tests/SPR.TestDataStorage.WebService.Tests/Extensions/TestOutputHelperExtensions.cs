using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace SPR.TestDataStorage.WebService.Tests.Extensions;

public static class TestOutputHelperExtensions
{
    public static async Task WriteContentBodyAsync(this ITestOutputHelper outputHelper, HttpResponseMessage response)
    {
        var contentBody = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(contentBody))
            return;

        try
        {
            outputHelper.WriteLine(JToken.Parse(contentBody).ToString());
        } catch(JsonException)
        {
            outputHelper.WriteLine(contentBody);
        }
    }
}

