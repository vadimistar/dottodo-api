using System.Linq.Expressions;

namespace dottodo.Auth;

public class TokenServiceException : Exception
{
    public TokenServiceException(string? message) : base(message)
    {
    }
}