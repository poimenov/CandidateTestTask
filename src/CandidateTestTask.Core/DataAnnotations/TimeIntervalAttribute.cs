using System.ComponentModel.DataAnnotations;
using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Core.DataAnnotations;

public class TimeIntervalAttribute : ValidationAttribute
{
    public TimeIntervalAttribute()
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is TimeIntervalDto timeInterval)
        {
            return timeInterval.StartTime.HasValue &&
                    timeInterval.EndTime.HasValue &&
                    timeInterval.StartTime < timeInterval.EndTime;
        }

        return false;
    }
}
