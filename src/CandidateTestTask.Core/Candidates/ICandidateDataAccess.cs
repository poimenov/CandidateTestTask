namespace CandidateTestTask.Core.Candidates;

public interface ICandidatesDataAccess
{
    Task<Candidate?> GetCandidateAsync(string email);
    Task CreateCandidateAsync(Candidate candidate);
    Task UpdateCandidateAsync(Candidate candidate);
    Task DeleteCandidateAsync(string email);
    Task<IEnumerable<Candidate>> GetCandidatesAsync(int skip, int take);
    Task<bool> IsCandidateExist(string email);
    Task<int> GetCountOfCandidatesAsync();
}
