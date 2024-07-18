using Microsoft.AspNetCore.Mvc.Testing;

namespace CandidateTestTask.Web.Host.Tests;

public class ProgramTests
{
    [Fact]
    public async Task TestRootEndpoint()
    {
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var response = await client.GetStringAsync("/");

        Assert.Equal("Hello World!", response);
    }
}
