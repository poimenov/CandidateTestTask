using AutoMapper;
using CandidateTestTask.Core.Candidates;
using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Application;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Candidate, CandidateDto>()
            .ForMember(x => x.TimeInterval, opt => opt.MapFrom(x => new TimeIntervalDto(x.StartTime, x.EndTime)));
        CreateMap<CandidateDto, Candidate>()
            .ForMember(x => x.StartTime, opt => opt.MapFrom(x => x.TimeInterval.StartTime))
            .ForMember(x => x.EndTime, opt => opt.MapFrom(x => x.TimeInterval.EndTime));
    }
}
