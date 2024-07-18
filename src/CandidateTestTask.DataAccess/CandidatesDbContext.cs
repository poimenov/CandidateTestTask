using CandidateTestTask.Core.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CandidateTestTask.DataAccess;

//cd src/CandidateTestTask.DataAccess
//dotnet ef migrations add InitialCreate
public class CandidatesDbContext : DbContext
{
    protected readonly string? _connectionString;
    public CandidatesDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public CandidatesDbContext(DbContextOptions<CandidatesDbContext> options) : base(options)
    {
        //
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_connectionString);
    }
    public virtual DbSet<Candidate> Candidates { get; set; }
}
