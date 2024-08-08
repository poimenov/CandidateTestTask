using Bogus;
using CandidateTestTask.Application.Candidates;
using CandidateTestTask.Application.Candidates.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace CandidateTestTask.Web.Host.Tests;

public class CandidatesEndPointTestsTests
{
    [Fact]
    public async Task GetCandidatesAsync__WhithPageSize_ShouldReturnCandidates()
    {
        // Arrange
        var count = 40;
        var page = 1;
        int? pageSize = 20;
        var expected = GetCandidates(pageSize.Value);
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.GetCountOfCandidatesAsync()).ReturnsAsync(count);
        candidatesServiceMock.Setup(x => x.GetCandidatesAsync(It.Is<int>(x => x == page), It.Is<int?>(x => x == pageSize))).ReturnsAsync(expected);
        // Act
        var result = await CandidatesEndPoint.GetCandidatesAsync(page, pageSize, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.GetCountOfCandidatesAsync(), Times.Once);
        candidatesServiceMock.Verify(x => x.GetCandidatesAsync(It.Is<int>(x => x == page), It.Is<int?>(x => x == pageSize)), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.IsType<Ok<CandidatesResult>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal(count, result.Value.TotalCount);
        Assert.Equal(expected, result.Value.CandidateDtos);
    }

    [Fact]
    public async Task GetCandidatesAsync__WhithOutPageSize_ShouldReturnCandidates()
    {
        // Arrange
        var count = 20;
        var page = 1;
        int? pageSize = 10;
        var expected = GetCandidates(pageSize.Value);
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.GetCountOfCandidatesAsync()).ReturnsAsync(count);
        candidatesServiceMock.Setup(x => x.GetCandidatesAsync(It.Is<int>(x => x == page), null)).ReturnsAsync(expected);
        // Act
        var result = await CandidatesEndPoint.GetCandidatesAsync(page, null, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.GetCountOfCandidatesAsync(), Times.Once);
        candidatesServiceMock.Verify(x => x.GetCandidatesAsync(It.Is<int>(x => x == page), It.Is<int?>(x => x == null)), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.IsType<Ok<CandidatesResult>>(result);
        Assert.NotNull(result.Value);
        Assert.Equal(count, result.Value.TotalCount);
        Assert.Equal(expected, result.Value.CandidateDtos);
    }

    [Fact]
    public async Task GetCountOfCandidatesAsync_ShouldReturnCount()
    {
        // Arrange
        var expected = 10;
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.GetCountOfCandidatesAsync()).ReturnsAsync(expected);
        // Act
        var result = await CandidatesEndPoint.GetCountOfCandidatesAsync(candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.GetCountOfCandidatesAsync(), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.IsType<Ok<int>>(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public async Task GetCandidateAsync_ShouldReturnCandidate()
    {
        // Arrange
        var expected = GetCandidates(1).First();
        var email = expected.Email;
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.GetCandidateAsync(It.Is<string>(x => x == email))).ReturnsAsync(expected);
        // Act
        var result = await CandidatesEndPoint.GetCandidateAsync(email, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.GetCandidateAsync(It.Is<string>(x => x == email)), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.IsType<Results<Ok<CandidateDto>, BadRequest, NotFound>>(result);
        var okResult = (Ok<CandidateDto>)result.Result;
        Assert.NotNull(okResult);
        Assert.Equal(expected, okResult.Value);
    }

    [Theory]
    [InlineData("bad_email.com")]
    [InlineData("@bad_email")]
    [InlineData("bad_email@")]
    public async Task GetCandidateAsync_ShouldReturnBadRequest(string email)
    {
        // Arrange
        var candidatesServiceMock = new Mock<ICandidatesService>();
        // Act
        var result = await CandidatesEndPoint.GetCandidateAsync(email, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.GetCandidateAsync(It.Is<string>(x => x == email)), Times.Never);
        Assert.IsType<Results<Ok<CandidateDto>, BadRequest, NotFound>>(result);
        var badResult = (BadRequest)result.Result;
        Assert.NotNull(badResult);
    }

    [Fact]
    public async Task GetCandidateAsync_ShouldReturnNotFound()
    {
        // Arrange
        var email = "not_found@email.com";
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.GetCandidateAsync(It.Is<string>(x => x == email))).ReturnsAsync(null as CandidateDto);
        // Act
        var result = await CandidatesEndPoint.GetCandidateAsync(email, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.GetCandidateAsync(It.Is<string>(x => x == email)), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.IsType<Results<Ok<CandidateDto>, BadRequest, NotFound>>(result);
        var notFoundResult = (NotFound)result.Result;
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task CreateUpdateCandidateAsync_ShouldReturnOk()
    {
        // Arrange
        var candidate = GetCandidates(1).First();
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.CreateUpdateCandidateAsync(It.Is<CandidateDto>(x => x == candidate)));
        // Act
        var result = await CandidatesEndPoint.CreateUpdateCandidateAsync(candidate, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.CreateUpdateCandidateAsync(It.Is<CandidateDto>(x => x == candidate)), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.NotNull(result);
        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldReturnOk()
    {
        // Arrange
        var email = "delete@email.com";
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.IsCandidateExist(It.Is<string>(x => x == email))).ReturnsAsync(true);
        candidatesServiceMock.Setup(x => x.DeleteCandidateAsync(It.Is<string>(x => x == email)));
        // Act
        var result = await CandidatesEndPoint.DeleteCandidateAsync(email, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.IsCandidateExist(It.Is<string>(x => x == email)), Times.Once);
        candidatesServiceMock.Verify(x => x.DeleteCandidateAsync(It.Is<string>(x => x == email)), Times.Once);
        candidatesServiceMock.VerifyNoOtherCalls();
        Assert.NotNull(result);
        Assert.IsType<Results<Ok, BadRequest, NotFound>>(result);
        var okResult = (Ok)result.Result;
        Assert.NotNull(okResult);
    }

    [Theory]
    [InlineData("bad_email.com")]
    [InlineData("@bad_email")]
    [InlineData("bad_email@")]
    public async Task DeleteCandidateAsync_ShouldReturnBadRequest(string email)
    {
        // Arrange
        var candidatesServiceMock = new Mock<ICandidatesService>();
        // Act
        var result = await CandidatesEndPoint.DeleteCandidateAsync(email, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.IsCandidateExist(It.Is<string>(x => x == email)), Times.Never);
        candidatesServiceMock.Verify(x => x.DeleteCandidateAsync(It.Is<string>(x => x == email)), Times.Never);
        Assert.NotNull(result);
        Assert.IsType<Results<Ok, BadRequest, NotFound>>(result);
        var badRequestResult = (BadRequest)result.Result;
        Assert.NotNull(badRequestResult);
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldReturnNotFound()
    {
        // Arrange
        var email = "delete@email.com";
        var candidatesServiceMock = new Mock<ICandidatesService>();
        candidatesServiceMock.Setup(x => x.IsCandidateExist(It.Is<string>(x => x == email))).ReturnsAsync(false);
        // Act
        var result = await CandidatesEndPoint.DeleteCandidateAsync(email, candidatesServiceMock.Object);
        // Assert
        candidatesServiceMock.Verify(x => x.IsCandidateExist(It.Is<string>(x => x == email)), Times.Once);
        candidatesServiceMock.Verify(x => x.DeleteCandidateAsync(It.Is<string>(x => x == email)), Times.Never);
        Assert.NotNull(result);
        Assert.IsType<Results<Ok, BadRequest, NotFound>>(result);
        var notFoundResult = (NotFound)result.Result;
        Assert.NotNull(notFoundResult);
    }

    private static IEnumerable<CandidateDto> GetCandidates(int count)
    {
        var StartTimeMin = new TimeOnly(8, 0, 0);
        var StartTimeMax = new TimeOnly(10, 0, 0);
        var EndTimeMin = new TimeOnly(17, 0, 0);
        var EndTimeMax = new TimeOnly(19, 0, 0);

        var timeIntervalFaker = new Faker<TimeIntervalDto>()
           .RuleFor(x => x.StartTime, f => f.Date.BetweenTimeOnly(StartTimeMin, StartTimeMax))
           .RuleFor(x => x.EndTime, f => f.Date.BetweenTimeOnly(EndTimeMin, EndTimeMax));

        var candidateFaker = new Faker<CandidateDto>()
        .RuleFor(x => x.Email, f => f.Internet.Email())
        .RuleFor(x => x.FirstName, f => f.Name.FirstName())
        .RuleFor(x => x.LastName, f => f.Name.LastName())
        .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(x => x.LinkedInUrl, f => f.Internet.Url())
        .RuleFor(x => x.GitHubUrl, f => f.Internet.Url())
        .RuleFor(x => x.Comment, f => f.Lorem.Paragraph())
        .RuleFor(x => x.TimeInterval, f => timeIntervalFaker.Generate());

        return candidateFaker.Generate(count);
    }

}
