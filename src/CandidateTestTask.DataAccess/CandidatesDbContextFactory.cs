using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CandidateTestTask.DataAccess;

public class CandidatesDbContextFactory : IDbContextFactory<CandidatesDbContext>
{
    private readonly IConfiguration _configuration;
    public CandidatesDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CandidatesDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CandidatesDbContext>()
            .UseSqlServer(_configuration.GetConnectionString("Default"))
            .Options;
        return new CandidatesDbContext(options);
    }
}
