using System.ComponentModel.DataAnnotations;
using CandidateTestTask.Core.Candidates.Dto;

namespace CandidateTestTask.Core.Extensions;

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
