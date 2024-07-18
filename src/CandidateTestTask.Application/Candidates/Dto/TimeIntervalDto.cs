namespace CandidateTestTask.Application.Candidates.Dto;

public struct TimeIntervalDto
{
    public TimeIntervalDto()
    {
        //
    }
    public TimeIntervalDto(TimeOnly? startTime, TimeOnly? endTime)
    {
        if (startTime.HasValue && endTime.HasValue && startTime.Value < endTime.Value)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }
}
