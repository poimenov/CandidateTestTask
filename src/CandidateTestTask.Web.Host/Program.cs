using CandidateTestTask.Application.Extensions;
using CandidateTestTask.Web.Host;

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

// candidates endpoints
app.MapGroup("/candidates/v1")
    .MapCandidatesApi()
    .WithTags("Candidates Endpoints");

app.Run();


public partial class Program { }
