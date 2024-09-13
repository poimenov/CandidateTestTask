using System.ComponentModel.DataAnnotations;
using CandidateTestTask.Core.Candidates;
using CandidateTestTask.Core.Candidates.Dto;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CandidateTestTask.Web.Host;

public static class CandidatesEndPoint
{
    public static RouteGroupBuilder MapCandidatesApi(this RouteGroupBuilder group)
    {
        group.MapGet("/candidates/{page}/{pageSize?}", GetCandidatesAsync);
        group.MapGet("/candidates/count", GetCountOfCandidatesAsync);
        group.MapGet("/candidate/{email}", GetCandidateAsync);
        group.MapPost("/candidate", CreateUpdateCandidateAsync).AddEndpointFilter<CandidateIsValidFilter>();
        group.MapDelete("/candidate/{email}", DeleteCandidateAsync);
        return group;
    }

    public static async Task<Ok<CandidatesResult>> GetCandidatesAsync(int page, int? pageSize, ICandidatesService candidates)
    {
        var retVal = new CandidatesResult()
        {
            CandidateDtos = await candidates.GetCandidatesAsync(((page < 1) ? 1 : page), pageSize),
            TotalCount = await candidates.GetCountOfCandidatesAsync()
        };

        return TypedResults.Ok(retVal);
    }

    public static async Task<Ok<int>> GetCountOfCandidatesAsync(ICandidatesService candidates)
    {
        var result = await candidates.GetCountOfCandidatesAsync();
        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<CandidateDto>, BadRequest, NotFound>> GetCandidateAsync(string email, ICandidatesService candidates)
    {
        var att = new EmailAddressAttribute();
        if (!att.IsValid(email))
        {
            return TypedResults.BadRequest();
        }

        var candidate = await candidates.GetCandidateAsync(email);
        return candidate != null ? TypedResults.Ok(candidate) : TypedResults.NotFound();
    }

    public static async Task<Ok> CreateUpdateCandidateAsync(CandidateDto candidate, ICandidatesService candidates)
    {
        await candidates.CreateUpdateCandidateAsync(candidate);
        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, BadRequest, NotFound>> DeleteCandidateAsync(string email, ICandidatesService candidates)
    {
        var att = new EmailAddressAttribute();
        if (!att.IsValid(email))
        {
            return TypedResults.BadRequest();
        }

        if (!await candidates.IsCandidateExist(email))
        {
            return TypedResults.NotFound();
        }

        await candidates.DeleteCandidateAsync(email);
        return TypedResults.Ok();
    }
}
