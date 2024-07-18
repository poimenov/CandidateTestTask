using System.ComponentModel.DataAnnotations;
using CandidateTestTask.Application.Candidates.Dto;

namespace CandidateTestTask.Application.Extensions;

public static class CandidateDtoExtension
{
    public static (bool IsValid, ICollection<ValidationResult>? ValidationResults) IsValid(this CandidateDto obj)
    {
        ICollection<ValidationResult> validationResults = new List<ValidationResult>();
        if (obj != null && !Validator.TryValidateObject(obj!, new ValidationContext(obj!), validationResults, true))
        {
            return (false, validationResults);
        }

        return (true, null);
    }
}
