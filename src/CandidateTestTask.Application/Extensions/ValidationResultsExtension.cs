using System.ComponentModel.DataAnnotations;

namespace CandidateTestTask.Application.Extensions;

public static class ValidationResultsExtension
{
    public static Dictionary<string, string[]> ToErrorsDictionary(this IEnumerable<ValidationResult> validationResults)
    {
        var errors = new Dictionary<string, string[]>();
        foreach (var validationResult in validationResults)
        {
            foreach (var name in validationResult.MemberNames)
            {
                if (errors.ContainsKey(name))
                {
                    errors[name].Append(validationResult.ErrorMessage);
                }
                else if (!string.IsNullOrEmpty(validationResult.ErrorMessage))
                {
                    errors.Add(name, new[] { validationResult.ErrorMessage });
                }
            }
        }

        return errors;
    }
}
