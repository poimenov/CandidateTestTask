using System.Net;
using System.Net.Http.Json;
using CandidateTestTask.Core.Candidates.Dto;
using Microsoft.AspNetCore.Http;

namespace CandidateTestTask.Web.Host.Tests;

[Collection("Sequential")]
public class CandidateIsValidFilterTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public CandidateIsValidFilterTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }
    public static IEnumerable<object[]> InvalidCandidates => new List<object[]>
    {
        new object[] { new CandidateDto { Email = "test1_example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The Email field is not a valid e-mail address." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = null,            LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The FirstName field is required." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = null,           PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The LastName field is required." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "abcdefghjk", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The PhoneNumber field is not a valid phone number." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = null,           GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The Comment field is required." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "github.com/johndoe",         LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The GitHubUrl field is not a valid fully-qualified http, https, or ftp URL." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "linkedin.com/in/johndoe",             TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)) }, "The LinkedInUrl field is not a valid fully-qualified http, https, or ftp URL." },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(null, new TimeOnly(17, 0)) }, "Invalid time interval" },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), null) }, "Invalid time interval" },
        new object[] { new CandidateDto { Email = "test1@example.com", FirstName = "TestFirstName", LastName = "TestLastName", PhoneNumber = "1234567890", Comment = "Test comment", GitHubUrl = "https://github.com/johndoe", LinkedInUrl = "https://www.linkedin.com/in/johndoe", TimeInterval = new TimeIntervalDto(new TimeOnly(17, 0), new TimeOnly(9, 0)) }, "Invalid time interval" },
    };

    [Theory]
    [MemberData(nameof(InvalidCandidates))]
    public async Task PostCandidateWithValidationProblems_ShouldReturnsBadRequest(CandidateDto candidate, string errorMessage)
    {
        var response = await _httpClient.PostAsJsonAsync("/candidates/v1/candidate", candidate);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.NotNull(problemResult?.Errors);
        Assert.Collection(problemResult.Errors, (error) => Assert.Equal(errorMessage, error.Value.First()));
    }

    [Fact]
    public async Task PostCandidateWithMultipleValidationProblems_ShouldReturnsBadRequest()
    {
        var candidate = new CandidateDto
        {
            Email = "test1_example.com",
            FirstName = null,
            LastName = null,
            PhoneNumber = "qwerty",
            Comment = null,
            GitHubUrl = "github.com/johndoe",
            LinkedInUrl = "linkedin.com/in/johndoe",
            TimeInterval = new TimeIntervalDto(new TimeOnly(17, 0), new TimeOnly(9, 0)),
        };

        var response = await _httpClient.PostAsJsonAsync("/candidates/v1/candidate", candidate);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(problemResult);
        Assert.NotNull(problemResult.Errors);
        Assert.Equal(8, problemResult.Errors.Count);
        var errorMessages = InvalidCandidates.AsQueryable<object[]>().Select(x => (string)x[1]).Distinct().ToList();
        Assert.Equal(errorMessages.Order(), problemResult.Errors.SelectMany(x => x.Value).Distinct().Order());
    }

    [Fact]
    public async Task PostValidCandidate_ShouldReturnsOk()
    {
        var candidate = new CandidateDto
        {
            Email = "test1@example.com",
            FirstName = "Test",
            LastName = "Test",
            PhoneNumber = "1234567890",
            Comment = "Test comment",
            GitHubUrl = "https://github.com/johndoe",
            LinkedInUrl = "https://www.linkedin.com/in/johndoe",
            TimeInterval = new TimeIntervalDto(new TimeOnly(9, 0), new TimeOnly(17, 0)),
        };

        var response = await _httpClient.PostAsJsonAsync("/candidates/v1/candidate", candidate);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
