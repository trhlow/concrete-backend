using Concrete.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Concrete.Api.Security;

public class PasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool Verify(User user, string password)
    {
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}
