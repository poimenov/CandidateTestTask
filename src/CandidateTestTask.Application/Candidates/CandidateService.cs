using AutoMapper;
using CandidateTestTask.Application.Candidates.Dto;
using CandidateTestTask.Core;
using CandidateTestTask.Core.Candidates;
using Microsoft.Extensions.Options;

namespace CandidateTestTask.Application.Candidates;

public class CandidatesService : ICandidatesService
{
    private readonly IMapper _mapper;
    protected readonly CandidatesOptions _options;
    protected readonly ICandidatesDataAccess _candidateDataAccess;
    public CandidatesService(IMapper mapper, IOptionsMonitor<CandidatesOptions> options, ICandidatesDataAccess candidateDataAccess)
    {
        _mapper = mapper;
        _options = options.CurrentValue;
        _candidateDataAccess = candidateDataAccess;
    }

    public async Task CreateUpdateCandidateAsync(CandidateDto candidate)
    {
        if (await _candidateDataAccess.IsCandidateExist(candidate.Email))
        {
            await _candidateDataAccess.UpdateCandidateAsync(_mapper.Map<Candidate>(candidate));
        }
        else
        {
            await _candidateDataAccess.CreateCandidateAsync(_mapper.Map<Candidate>(candidate));
        }
    }

    public async Task DeleteCandidateAsync(string email)
    {
        await _candidateDataAccess.DeleteCandidateAsync(email);
    }

    public async Task<CandidateDto?> GetCandidateAsync(string email)
    {
        return await _candidateDataAccess.GetCandidateAsync(email)
                                        .ContinueWith(task => _mapper.Map<CandidateDto?>(task.Result));
    }

    public async Task<IEnumerable<CandidateDto>> GetCandidatesAsync(int page)
    {
        return await _candidateDataAccess.GetCandidatesAsync((_options.PageSize * (page - 1)), _options.PageSize)
                                        .ContinueWith(task => _mapper.Map<IEnumerable<CandidateDto>>(task.Result));
    }

    public async Task<int> GetCountOfCandidatesAsync()
    {
        return await _candidateDataAccess.GetCountOfCandidatesAsync();
    }

    public async Task<bool> IsCandidateExist(string email)
    {
        return await _candidateDataAccess.IsCandidateExist(email);
    }
}
