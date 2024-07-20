using System.Diagnostics.CodeAnalysis;
using CandidateTestTask.Core.Candidates;

namespace CandidateTestTask.DataAccess.Tests.Candidates;

public class CandidateComparer : IEqualityComparer<Candidate?>
{
    public bool Equals(Candidate? x, Candidate? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        return x.Email == y.Email
            && x.FirstName == y.FirstName
            && x.LastName == y.LastName
            && x.Comment == y.Comment;
    }

    public int GetHashCode([DisallowNull] Candidate? obj)
    {
        return obj.Email.GetHashCode() ^
                obj.FirstName.GetHashCode() ^
                obj.LastName.GetHashCode() ^
                obj.Comment.GetHashCode();
    }
}
