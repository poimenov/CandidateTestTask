using CandidateTestTask.Core.Candidates;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CandidateTestTask.Web.Host.Tests;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICandidatesService));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddTransient<ICandidatesService, TestCandidatesService>();
        });

        return base.CreateHost(builder);
    }
}
