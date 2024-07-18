using CandidateTestTask.Core.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CandidateTestTask.DataAccess;

public class CandidatesDataAccess : ICandidatesDataAccess
{
    protected readonly IConfiguration _configuration;
    public CandidatesDataAccess(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task CreateCandidateAsync(Candidate candidate)
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            db.Candidates.Add(candidate);
            await db.SaveChangesAsync();
        }
    }

    public async Task DeleteCandidateAsync(string email)
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            if (db.Candidates.Any(c => c.Email == email))
            {
                var candidate = db.Candidates.First(c => c.Email == email);
                db.Candidates.Remove(candidate);
                await db.SaveChangesAsync();
            }
        }
    }

    public async Task<Candidate?> GetCandidateAsync(string email)
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            return await db.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }
    }

    public async Task<IEnumerable<Candidate>> GetCandidatesAsync(int skip, int take)
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            return await db.Candidates.Skip(skip).Take(take).ToListAsync();
        }
    }

    public async Task<int> GetCountOfCandidatesAsync()
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            return await db.Candidates.CountAsync();
        }
    }

    public bool IsCandidateExist(string email)
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            return db.Candidates.Any(c => c.Email == email);
        }
    }

    public async Task UpdateCandidateAsync(Candidate candidate)
    {
        using (var db = new CandidatesDbContext(_configuration))
        {
            if (db.Candidates.Any(c => c.Email == candidate.Email))
            {
                var c = db.Candidates.First(c => c.Email == candidate.Email);
                c.FirstName = candidate.FirstName;
                c.LastName = candidate.LastName;
                c.PhoneNumber = candidate.PhoneNumber;
                c.LinkedInUrl = candidate.LinkedInUrl;
                c.GitHubUrl = candidate.GitHubUrl;
                c.Comment = candidate.Comment;
                c.StartTime = candidate.StartTime;
                c.EndTime = candidate.EndTime;
                await db.SaveChangesAsync();
            }
        }
    }
}
