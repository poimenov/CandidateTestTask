using CandidateTestTask.Application.Extensions;
using CandidateTestTask.Web.Host;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCandidatesConfiguration(builder.Configuration)
                .AddCandidatesService();


// App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
var origins = (builder.Configuration["App:CorsOrigins"] ?? "http://localhost:5173")
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
// Configure CORS for react UI
var defaultCorsPolicyName = "localhost";
builder.Services.AddCors(
    options => options.AddPolicy(
        defaultCorsPolicyName,
        builder => builder
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    )
);

var app = builder.Build();
app.UseCors(defaultCorsPolicyName);

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
