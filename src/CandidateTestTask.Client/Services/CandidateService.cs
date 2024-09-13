using System.Net.Http.Json;
using CandidateTestTask.Client.Models;
using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Client.Services;

public class CandidateService : ICandidateService
{
    private const string prefix = "/candidates/v1";
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public CandidateService(IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<CandidatesResponce?> GetCandidatesAsync(int page, int pageSize) =>
        await _httpClient.GetFromJsonAsync<CandidatesResponce>($"{prefix}/candidates/{page}/{pageSize}");

    public async Task<int?> GetCountOfCandidatesAsync(string email) =>
        await _httpClient.GetFromJsonAsync<int?>($"{prefix}/candidates/count");

    public async Task<CandidateDto?> GetCandidateAsync(string email) =>
        await _httpClient.GetFromJsonAsync<CandidateDto>($"{prefix}/candidate/{email}");

    public async Task<HttpResponseMessage> CreateUpdateCandidateAsync(CandidateDto candidate) =>
        await _httpClient.PostAsJsonAsync($"{prefix}/candidate", candidate);

    public async Task<HttpResponseMessage> DeleteCandidateAsync(string email) =>
        await _httpClient.DeleteAsync($"{prefix}/candidate/{email}");
}
