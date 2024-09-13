using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Core.Candidates;

public interface ICandidatesService
{
    Task CreateUpdateCandidateAsync(CandidateDto candidate);
    Task DeleteCandidateAsync(string email);
    Task<CandidateDto?> GetCandidateAsync(string email);
    Task<IEnumerable<CandidateDto>> GetCandidatesAsync(int page, int? pageSize);
    Task<int> GetCountOfCandidatesAsync();
    Task<bool> IsCandidateExist(string email);
}
