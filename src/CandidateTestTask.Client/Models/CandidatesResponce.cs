using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Client.Models;

public class CandidatesResponce
{
    public CandidateDto[] CandidateDtos { get; set; }
    public int TotalCount { get; set; }
}
