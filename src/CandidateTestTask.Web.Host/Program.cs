using System.ComponentModel.DataAnnotations;
using CandidateTestTask.Application.Candidates;
using CandidateTestTask.Application.Candidates.Dto;
using CandidateTestTask.Application.Extensions;
using CandidateTestTask.Web.Host;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCandidatesConfiguration(builder.Configuration)
                .AddCandidatesService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World");
app.MapGet("/candidates/{page?}", async ([FromRoute] int? page, [FromServices] ICandidatesService candidates) =>
                                                                    await candidates.GetCandidatesAsync(page ?? 0));

app.MapGet("/candidates/count", async ([FromServices] ICandidatesService candidates) =>
                                                                    await candidates.GetCountOfCandidatesAsync());

app.MapGet("/candidate/{email}", 
    async (string email, ICandidatesService candidates) =>
    {
        var att = new EmailAddressAttribute();
        if (!att.IsValid(email))
        {
            return Results.BadRequest();
        }

        var candidate = await candidates.GetCandidateAsync(email);
        return candidate != null ? Results.Ok(candidate) : Results.NotFound();
    });

app.MapPost("/candidate",
    async ([FromBody] CandidateDto candidate, [FromServices] ICandidatesService candidates) =>
    {
        await candidates.CreateUpdateCandidateAsync(candidate);
        return Results.NoContent();
    }).AddEndpointFilter<CandidateIsValidFilter>();

app.MapDelete("/candidate/{email}",
    async ([FromRoute] string email, [FromServices] ICandidatesService candidates) =>
    {
        var att = new EmailAddressAttribute();
        if (!att.IsValid(email))
        {
            return Results.BadRequest();
        }

        if (!candidates.IsCandidateExist(email))
        {
            return Results.NotFound();
        }

        await candidates.DeleteCandidateAsync(email);
        return Results.Ok();
    });

app.Run();
