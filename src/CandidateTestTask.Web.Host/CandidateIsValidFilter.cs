using CandidateTestTask.Application.Candidates.Dto;
using CandidateTestTask.Application.Extensions;

namespace CandidateTestTask.Web.Host;

public class CandidateIsValidFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var candidate = context.GetArgument<CandidateDto>(0);

        var result = candidate.IsValid();

        if (!result.IsValid && result.ValidationResults != null)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var validationResult in result.ValidationResults)
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

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
