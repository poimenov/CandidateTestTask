﻿using CandidateTestTask.DataAccess;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using CandidateTestTask.Core.Candidates;
using Bogus;

public class Program
{
    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
    public static void Main(string[] args)
    {
        try
        {
            XmlConfigurator.Configure(File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
            // var variables = Environment.GetEnvironmentVariables()
            //     .Cast<DictionaryEntry>().OrderBy(x => x.Key).ThenBy(x => x.Value).Select(x => $"{x.Key} : {x.Value}").ToArray();
            // _log.Info($"Environment variables:{Environment.NewLine} {String.Join(Environment.NewLine, variables)}");
            var environmentName = string.Empty;
            var consoleEnvironment = Environment.GetEnvironmentVariable("CONSOLE_ENVIRONMENT");
            if (!string.IsNullOrEmpty(consoleEnvironment))
            {
                environmentName = $".{consoleEnvironment}";
            }

            var jsonFileName = $"appsettings{environmentName}.json";
            var configuration = new ConfigurationBuilder().AddJsonFile(jsonFileName).Build();
            Migrate(configuration);

            if (args.Contains("fakedata"))
            {
                AddFakeCandidates(configuration);
            }

            _log.Info("Database migrated");
        }
        catch (System.Exception ex)
        {
            _log.Info("Database not migrated");
            _log.Error(ex.Message, ex);
            Console.WriteLine("Database not migrated");
            Console.WriteLine(ex.Message);
        }

    }

    public static void Migrate(IConfigurationRoot configuration)
    {
        using (var context = new CandidatesDbContext(configuration))
        {
            context.Database.Migrate();
        }
    }

    public static void AddFakeCandidates(IConfigurationRoot configuration, int count = 100)
    {
        var dataAccess = new CandidatesDataAccess(configuration);
        GetCandidates(count).ToList().ForEach(x => dataAccess.CreateCandidateAsync(x).GetAwaiter().GetResult());
    }

    public static IEnumerable<Candidate> GetCandidates(int count)
    {
        var candidateFaker = new Faker<Candidate>()
        .RuleFor(x => x.Email, f => f.Internet.Email())
        .RuleFor(x => x.FirstName, f => f.Name.FirstName())
        .RuleFor(x => x.LastName, f => f.Name.LastName())
        .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(x => x.LinkedInUrl, f => f.Internet.Url())
        .RuleFor(x => x.GitHubUrl, f => f.Internet.Url())
        .RuleFor(x => x.Comment, f => f.Lorem.Paragraph());

        return candidateFaker.Generate(count);
    }
}
