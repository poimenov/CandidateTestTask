using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CandidateTestTask.DataAccess;

public class CandidatesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CandidatesDbContext>
{
    public CandidatesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CandidatesDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost; Database=CandidatetestTaskDb; Trusted_Connection=True;");

        return new CandidatesDbContext(optionsBuilder.Options);
    }
}
