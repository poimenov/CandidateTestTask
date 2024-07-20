using AutoMapper;
using Bogus;
using CandidateTestTask.Application.Candidates;
using CandidateTestTask.Application.Candidates.Dto;
using CandidateTestTask.Core;
using CandidateTestTask.Core.Candidates;
using Microsoft.Extensions.Options;
using Moq;

namespace CandidateTestTask.Application.Tests;

public class CandidateServiceTests
{
    [Fact]
    public async Task GetCandidatesAsync_ShouldReturnCandidates()
    {
        // Arrange
        var page = 1;
        var options = new CandidatesOptions
        {
            PageSize = 10
        };
        var candidates = GetCandidates(options.PageSize);
        var expectedCandidates = GetCandidateDtos(candidates);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<IEnumerable<CandidateDto>>(It.Is<IEnumerable<Candidate>>(x =>
                        Enumerable.SequenceEqual<Candidate>(x, candidates)))).Returns(expectedCandidates);

        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();
        optionsMock.Setup(x => x.CurrentValue).Returns(options);

        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.GetCandidatesAsync(It.Is<int>(x => x == ((page - 1) * options.PageSize)),
                                                                It.Is<int>(x => x == options.PageSize))).ReturnsAsync(candidates);

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);

        // Act
        var result = await candidatesService.GetCandidatesAsync(page);

        // Assert
        Assert.Equal(expectedCandidates, result);
        candidatesDataAccessMock.Verify(x => x.GetCandidatesAsync(It.Is<int>(x => x == ((page - 1) * options.PageSize)),
                                                                  It.Is<int>(x => x == options.PageSize)), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
        mapperMock.Verify(x => x.Map<IEnumerable<CandidateDto>>(It.Is<IEnumerable<Candidate>>(x =>
                                    Enumerable.SequenceEqual<Candidate>(x, candidates))), Times.Once);
        mapperMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetCandidateAsync_ShouldReturnCandidate()
    {
        // Arrange
        var candidates = GetCandidates(1);
        var candidate = candidates.First();
        var email = candidate.Email;
        var expectedCandidate = GetCandidateDtos(candidates).First();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<CandidateDto>(It.Is<Candidate>(x =>
                        x.Equals(candidate)))).Returns(expectedCandidate);

        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();

        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.GetCandidateAsync(It.Is<string>(x => x == email))).ReturnsAsync(candidate);

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);

        // Act
        var result = await candidatesService.GetCandidateAsync(email);

        // Assert
        Assert.Equal(expectedCandidate, result);
        candidatesDataAccessMock.Verify(x => x.GetCandidateAsync(It.Is<string>(x => x == email)), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
        mapperMock.Verify(x => x.Map<CandidateDto>(It.Is<Candidate>(x => x.Equals(candidate))), Times.Once);
        mapperMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateUpdateCandidateAsync_ShouldCreateCandidate()
    {
        // Arrange
        var candidates = GetCandidates(1);
        var candidate = candidates.First();
        var candidateDto = GetCandidateDtos(candidates).First();
        var email = candidate.Email;

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<Candidate>(It.Is<CandidateDto>(x =>
                        x.Equals(candidateDto)))).Returns(candidate);

        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();

        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.IsCandidateExist(It.Is<string>(x => x == email))).ReturnsAsync(false);
        candidatesDataAccessMock.Setup(x => x.CreateCandidateAsync(It.Is<Candidate>(x => x.Equals(candidate))));

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);
        // Act
        await candidatesService.CreateUpdateCandidateAsync(candidateDto);
        // Assert
        candidatesDataAccessMock.Verify(x => x.IsCandidateExist(It.Is<string>(x => x == email)), Times.Once);
        candidatesDataAccessMock.Verify(x => x.CreateCandidateAsync(It.Is<Candidate>(x => x.Equals(candidate))), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
        mapperMock.Verify(x => x.Map<Candidate>(It.Is<CandidateDto>(x => x.Equals(candidateDto))), Times.Once);
        mapperMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateUpdateCandidateAsync_ShouldUpdateCandidate()
    {
        // Arrange
        var candidates = GetCandidates(1);
        var candidate = candidates.First();
        var candidateDto = GetCandidateDtos(candidates).First();
        var email = candidate.Email;

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<Candidate>(It.Is<CandidateDto>(x =>
                        x.Equals(candidateDto)))).Returns(candidate);

        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();

        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.IsCandidateExist(It.Is<string>(x => x == email))).ReturnsAsync(true);
        candidatesDataAccessMock.Setup(x => x.UpdateCandidateAsync(It.Is<Candidate>(x => x.Equals(candidate))));

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);
        // Act
        await candidatesService.CreateUpdateCandidateAsync(candidateDto);
        // Assert
        candidatesDataAccessMock.Verify(x => x.IsCandidateExist(It.Is<string>(x => x == email)), Times.Once);
        candidatesDataAccessMock.Verify(x => x.UpdateCandidateAsync(It.Is<Candidate>(x => x.Equals(candidate))), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
        mapperMock.Verify(x => x.Map<Candidate>(It.Is<CandidateDto>(x => x.Equals(candidateDto))), Times.Once);
        mapperMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteCandidateAsync_ShouldDeleteCandidate()
    {
        // Arrange
        var email = "test@test.com";
        var mapperMock = new Mock<IMapper>();
        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();
        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.DeleteCandidateAsync(It.Is<string>(x => x == email)));

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);
        // Act
        await candidatesService.DeleteCandidateAsync(email);
        // Assert
        candidatesDataAccessMock.Verify(x => x.DeleteCandidateAsync(It.Is<string>(x => x == email)), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async void IsCandidateExist_ShouldReturnIsCandidateExist()
    {
        // Arrange
        var email = "test@test.com";
        var isExist = true;
        var mapperMock = new Mock<IMapper>();
        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();
        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.IsCandidateExist(It.Is<string>(x => x == email))).ReturnsAsync(isExist);

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);

        // Act
        var result = await candidatesService.IsCandidateExist(email);
        // Assert
        Assert.Equal(isExist, result);
        candidatesDataAccessMock.Verify(x => x.IsCandidateExist(It.Is<string>(x => x == email)), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetCountOfCandidatesAsync_ShouldReturnCountOfCandidates()
    {
        // Arrange
        var count = 100;
        var mapperMock = new Mock<IMapper>();
        var optionsMock = new Mock<IOptionsMonitor<CandidatesOptions>>();
        var candidatesDataAccessMock = new Mock<ICandidatesDataAccess>();
        candidatesDataAccessMock.Setup(x => x.GetCountOfCandidatesAsync()).ReturnsAsync(count);

        var candidatesService = new CandidatesService(mapperMock.Object, optionsMock.Object, candidatesDataAccessMock.Object);
        // Act
        var result = await candidatesService.GetCountOfCandidatesAsync();
        // Assert
        Assert.Equal(count, result);
        candidatesDataAccessMock.Verify(x => x.GetCountOfCandidatesAsync(), Times.Once);
        candidatesDataAccessMock.VerifyNoOtherCalls();
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

    private static List<CandidateDto> GetCandidateDtos(IEnumerable<Candidate> candidates)
    {
        var candidateDtos = new List<CandidateDto>();
        foreach (var candidate in candidates)
        {
            candidateDtos.Add(new CandidateDto
            {
                Email = candidate.Email,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Comment = candidate.Comment,
                PhoneNumber = candidate.PhoneNumber,
                LinkedInUrl = candidate.LinkedInUrl,
                GitHubUrl = candidate.GitHubUrl,
                TimeInterval = new TimeIntervalDto(candidate.StartTime, candidate.EndTime)
            });
        }
        return candidateDtos;
    }
}
