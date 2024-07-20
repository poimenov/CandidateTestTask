using CandidateTestTask.Core.Candidates;
using Microsoft.EntityFrameworkCore;

namespace CandidateTestTask.DataAccess.Tests.Candidates;

[Collection("CandidatesDatabaseCollection")]
public class CandidatesDataAccessTests
{
    DatabaseFixture _fixture;
    public CandidatesDataAccessTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetCandidateAsync_ShouldReturnCandidate()
    {
        // Arrange
        var email = _fixture.Emails.First();
        var expectedCandidate = _fixture.GetCandidate(email);

        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        var result = await dataAccess.GetCandidateAsync(email);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCandidate, result, new CandidateComparer());
        Assert.Equal(expectedCandidate?.PhoneNumber, result.PhoneNumber);
        Assert.Equal(expectedCandidate?.LinkedInUrl, result.LinkedInUrl);
        Assert.Equal(expectedCandidate?.GitHubUrl, result.GitHubUrl);
        Assert.Equal(expectedCandidate?.StartTime, result.StartTime);
        Assert.Equal(expectedCandidate?.EndTime, result.EndTime);
    }

    [Fact]
    public async Task GetCandidateAsync_ShouldReturnNull()
    {
        // Arrange
        var email = "non-existent@candidate.com";

        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        var result = await dataAccess.GetCandidateAsync(email);
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCandidatesAsync_ShouldReturnCandidates()
    {
        //Arange
        var take = 10;
        var skip = 10;
        var expectedCandidates = _fixture.GetCandidates(skip, take);
        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        var result = await dataAccess.GetCandidatesAsync(skip, take);
        // Assert
        Assert.Equal(expectedCandidates, result, new CandidateComparer());
    }

    [Fact]
    public async Task CreateCandidateAsync_ShouldCreateCandidate()
    {
        // Arrange
        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "CreateCandidateAsync@ShouldCreateCandidate.com",
            PhoneNumber = "1234567890",
            Comment = "Test comment",
            GitHubUrl = "https://github.com/CandidateTest/CandidateTestTask",
            LinkedInUrl = "https://www.linkedin.com/in/CandidateTest/",
            StartTime = new TimeOnly(8, 0, 0),
            EndTime = new TimeOnly(17, 0, 0)
        };
        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        await dataAccess.CreateCandidateAsync(candidate);
        // Assert
        var result = _fixture.GetCandidate(candidate.Email);
        Assert.NotNull(result);
        Assert.Equal(candidate, result, new CandidateComparer());
        Assert.Equal(candidate?.PhoneNumber, result.PhoneNumber);
        Assert.Equal(candidate?.LinkedInUrl, result.LinkedInUrl);
        Assert.Equal(candidate?.GitHubUrl, result.GitHubUrl);
        Assert.Equal(candidate?.StartTime, result.StartTime);
        Assert.Equal(candidate?.EndTime, result.EndTime);
    }

    [Fact]
    public async Task UpdateCandidateAsync_ShouldUpdateCandidate()
    {
        // Arrange
        var email = _fixture.Emails.First();
        var candidate = _fixture.GetCandidate(email);
        candidate!.FirstName = "Alexander";
        candidate!.LastName = "Poimenov";
        candidate!.PhoneNumber = "657535411";
        candidate!.LinkedInUrl = "https://www.linkedin.com/poimenov/";
        candidate!.GitHubUrl = "https://github.com/poimenov";
        candidate!.StartTime = new TimeOnly(10, 10, 0);
        candidate!.EndTime = new TimeOnly(16, 20, 0);
        candidate!.Comment = "Test comment updated";

        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        await dataAccess.UpdateCandidateAsync(candidate);
        // Assert
        var result = _fixture.GetCandidate(email);
        Assert.NotNull(result);
        Assert.Equal(candidate, result, new CandidateComparer());
        Assert.Equal(candidate?.PhoneNumber, result.PhoneNumber);
        Assert.Equal(candidate?.LinkedInUrl, result.LinkedInUrl);
        Assert.Equal(candidate?.GitHubUrl, result.GitHubUrl);
        Assert.Equal(candidate?.StartTime, result.StartTime);
        Assert.Equal(candidate?.EndTime, result.EndTime);
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldDeleteCandidate()
    {
        // Arrange
        var email = _fixture.Emails.Last();
        bool before;
        using (var db = _fixture.DbContextFactory.CreateDbContext())
        {
            before = await db.Candidates.AnyAsync(c => c.Email == email);
        }

        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        await dataAccess.DeleteCandidateAsync(email);
        // Assert
        using (var db = _fixture.DbContextFactory.CreateDbContext())
        {
            bool after = await db.Candidates.AnyAsync(c => c.Email == email);
            Assert.False(after);
        }

        Assert.True(before);
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldNotDeleteCandidate()
    {
        // Arrange
        var email = "non-existent@candidate.com";
        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        await dataAccess.DeleteCandidateAsync(email);
        // Assert
        using (var db = _fixture.DbContextFactory.CreateDbContext())
        {
            var result = await db.Candidates.AnyAsync(c => c.Email == email);
            Assert.False(result);
        }
    }

    [Fact]
    public async Task GetCountOfCandidatesAsync_ShouldReturnCountOfCandidates()
    {
        // Arrange
        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        var result = await dataAccess.GetCountOfCandidatesAsync();
        // Assert
        using (var db = _fixture.DbContextFactory.CreateDbContext())
        {
            var expectedCount = await db.Candidates.CountAsync();
            Assert.Equal(expectedCount, result);
        }
    }

    [Fact]
    public async Task IsCandidateExist_ShouldReturnTrue()
    {
        // Arrange
        var email = _fixture.Emails.First();
        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        var result = await dataAccess.IsCandidateExist(email);
        // Assert
        Assert.True(result);
    }
}
