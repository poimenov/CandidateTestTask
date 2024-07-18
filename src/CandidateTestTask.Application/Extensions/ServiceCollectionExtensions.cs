using CandidateTestTask.Application.Candidates;
using CandidateTestTask.Core;
using CandidateTestTask.Core.Candidates;
using CandidateTestTask.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CandidateTestTask.Application.Extensions;

public static class ServiceCollectionExtensions
{


    public static IServiceCollection AddCandidatesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CandidatesOptions>(configuration.GetSection(CandidatesOptions.SectionName));
        return services;
    }

    public static IServiceCollection AddCandidatesService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AppMappingProfile));
        services.AddTransient<ICandidatesDataAccess, CandidatesDataAccess>();
        services.AddTransient<ICandidatesService, CandidatesService>();
        return services;
    }
}
