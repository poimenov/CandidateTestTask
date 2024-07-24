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
            return Results.ValidationProblem(result.ValidationResults.ToErrorsDictionary());
        }

        return await next(context);
    }
}
