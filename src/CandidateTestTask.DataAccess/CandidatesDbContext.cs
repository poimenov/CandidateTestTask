using CandidateTestTask.Core.Candidates;
using Microsoft.EntityFrameworkCore;

namespace CandidateTestTask.DataAccess;

//cd src/CandidateTestTask.DataAccess
//dotnet ef migrations add InitialCreate
public class CandidatesDbContext : DbContext
{
    public CandidatesDbContext()
    {
        //
    }

    public CandidatesDbContext(DbContextOptions<CandidatesDbContext> options) : base(options)
    {
        //
    }

    public virtual DbSet<Candidate> Candidates { get; set; }
}
