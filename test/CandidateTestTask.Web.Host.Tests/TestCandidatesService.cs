using CandidateTestTask.Application.Candidates;
using CandidateTestTask.Application.Candidates.Dto;

namespace CandidateTestTask.Web.Host.Tests;

public class TestCandidatesService : ICandidatesService
{
    public async Task CreateUpdateCandidateAsync(CandidateDto candidate)
    {
        await Task.CompletedTask;
    }

    public async Task DeleteCandidateAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<CandidateDto?> GetCandidateAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CandidateDto>> GetCandidatesAsync(int page)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCountOfCandidatesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsCandidateExist(string email)
    {
        throw new NotImplementedException();
    }
}
