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
        var email = _fixture.Emails[0];
        var expectedCandidate = _fixture.GetCandidate(email);

        var dataAccess = new CandidatesDataAccess(_fixture.DbContextFactory);
        // Act
        var result = await dataAccess.GetCandidateAsync(email);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCandidate, result, new CandidateComparer());
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


}
