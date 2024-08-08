using CandidateTestTask.Application.Candidates.Dto;

namespace CandidateTestTask.Web.Host;

public class CandidatesResult
{
    public CandidatesResult()
    {
        CandidateDtos = new List<CandidateDto>();
        TotalCount = 0;
    }

    public CandidatesResult(IEnumerable<CandidateDto> candidateDtos, int totalCount)
    {
        CandidateDtos = candidateDtos;
        TotalCount = totalCount;
    }

    public IEnumerable<CandidateDto> CandidateDtos { get; set; }
    public int TotalCount { get; set; }
}
