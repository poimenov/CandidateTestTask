using System.ComponentModel.DataAnnotations;

namespace CandidateTestTask.Application.Exceptions;

public class ValidationsException : Exception
{
    public ValidationsException(string? message, IEnumerable<ValidationResult> validationResults) : base(message)
    {
        ValidationResults = validationResults;
    }

    public IEnumerable<ValidationResult> ValidationResults { get; private set; }
}
