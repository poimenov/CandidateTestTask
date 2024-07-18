using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CandidateTestTask.Core.Candidates;

public class Candidate
{
    [Key]
    [ReadOnly(true)]
    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; }

    [StringLength(25, MinimumLength = 3)]
    public string? PhoneNumber { get; set; }

    [StringLength(250, MinimumLength = 10)]
    public string? LinkedInUrl { get; set; }

    [StringLength(250, MinimumLength = 10)]
    public string? GitHubUrl { get; set; }

    [Required]
    public string Comment { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }
}
