using CandidateTestTask.Application.Candidates.Dto;

namespace CandidateTestTask.Application.Candidates;

public interface ICandidatesService
{
    Task CreateUpdateCandidateAsync(CandidateDto candidate);
    Task DeleteCandidateAsync(string email);
    Task<CandidateDto?> GetCandidateAsync(string email);
    Task<IEnumerable<CandidateDto>> GetCandidatesAsync(int page);
    Task<int> GetCountOfCandidatesAsync();
    Task<bool> IsCandidateExist(string email);
}
