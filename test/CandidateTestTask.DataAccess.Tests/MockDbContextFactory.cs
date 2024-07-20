using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CandidateTestTask.DataAccess.Tests;

public class MockDbContextFactory : IDbContextFactory<CandidatesDbContext>
{
    public const string DB_FILE_NAME = "candidates.db";
    private readonly DbContextOptions<CandidatesDbContext> _options;
    public MockDbContextFactory()
    {
        _options = new DbContextOptionsBuilder<CandidatesDbContext>()
            .UseSqlite($"Data Source={DB_FILE_NAME}")
            .ReplaceService<IMigrationsSqlGenerator, CustomSqliteMigrationsSqlGenerator>()
            .Options;
    }

    public CandidatesDbContext CreateDbContext()
    {
        return new CandidatesDbContext(_options);
    }
}
