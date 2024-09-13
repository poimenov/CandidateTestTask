using System.ComponentModel.DataAnnotations;
using CandidateTestTask.Core.DataAnnotations;

namespace CandidateTestTask.Core.Candidates.Dto;

public class CandidateDto
{
    [Required]
    [EmailAddress]
    [StringLength(150, MinimumLength = 5)]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string LastName { get; set; }

    [Phone]
    [StringLength(25, MinimumLength = 3)]
    public string? PhoneNumber { get; set; }

    [Url]
    [StringLength(250, MinimumLength = 1)]
    public string? LinkedInUrl { get; set; }

    [Url]
    [StringLength(250, MinimumLength = 1)]
    public string? GitHubUrl { get; set; }

    [Required]
    public string Comment { get; set; }

    [TimeInterval(ErrorMessage = "Invalid time interval")]
    public TimeIntervalDto TimeInterval { get; set; }
}
