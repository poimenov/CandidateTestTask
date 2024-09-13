using CandidateTestTask.Client.Models;
using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Client.Services;

public interface ICandidateService
{
    public Task<CandidatesResponce?> GetCandidatesAsync(int page, int pageSize);
    public Task<int?> GetCountOfCandidatesAsync(string email);
    public Task<CandidateDto?> GetCandidateAsync(string email);
    public Task<HttpResponseMessage> CreateUpdateCandidateAsync(CandidateDto candidate);
    public Task<HttpResponseMessage> DeleteCandidateAsync(string email);
}
