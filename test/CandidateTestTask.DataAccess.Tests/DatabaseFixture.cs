using Bogus;
using CandidateTestTask.Core.Candidates;
using Microsoft.EntityFrameworkCore;

namespace CandidateTestTask.DataAccess.Tests;
public class DatabaseFixture
{
    IDbContextFactory<CandidatesDbContext> _dbContextFactory;
    public DatabaseFixture()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MockDbContextFactory.DB_FILE_NAME);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        _dbContextFactory = new MockDbContextFactory();
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.Database.Migrate();
            var candidates = GetCandidates(100);
            _emails = candidates.Select(x => x.Email).ToArray();
            context.Candidates.AddRange(candidates);
            context.SaveChanges();
        }
    }

    private string[] _emails;
    public string[] Emails => _emails;

    public CandidatesDbContext CreateContext() => _dbContextFactory.CreateDbContext();
    public IDbContextFactory<CandidatesDbContext> DbContextFactory => _dbContextFactory;
    public Candidate? GetCandidate(string email)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return context.Candidates.First(x => x.Email == email);
        }
    }

    public IEnumerable<Candidate> GetCandidates(int skip, int take)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return context.Candidates.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).Skip(skip).Take(take).ToList();
        }
    }

    private static IEnumerable<Candidate> GetCandidates(int count)
    {
        var StartTimeMin = new TimeOnly(8, 0, 0);
        var StartTimeMax = new TimeOnly(10, 0, 0);
        var EndTimeMin = new TimeOnly(17, 0, 0);
        var EndTimeMax = new TimeOnly(19, 0, 0);

        var candidateFaker = new Faker<Candidate>()
        .RuleFor(x => x.Email, f => f.Internet.Email())
        .RuleFor(x => x.FirstName, f => f.Name.FirstName())
        .RuleFor(x => x.LastName, f => f.Name.LastName())
        .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(x => x.LinkedInUrl, f => f.Internet.Url())
        .RuleFor(x => x.GitHubUrl, f => f.Internet.Url())
        .RuleFor(x => x.Comment, f => f.Lorem.Paragraph())
        .RuleFor(x => x.StartTime, f => f.Date.BetweenTimeOnly(StartTimeMin, StartTimeMax))
        .RuleFor(x => x.EndTime, f => f.Date.BetweenTimeOnly(EndTimeMin, EndTimeMax));

        return candidateFaker.Generate(count);
    }

}

[CollectionDefinition("CandidatesDatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}