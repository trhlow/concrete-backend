using Concrete.Api.Data;
using Concrete.Api.DTOs.Auth;
using Concrete.Api.Entities;
using Concrete.Api.Security;
using Microsoft.EntityFrameworkCore;

namespace Concrete.Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly PasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ITwoFactorCache _twoFactorCache;

    public AuthService(
        ApplicationDbContext db,
        PasswordHasher passwordHasher,
        ITokenService tokenService,
        ITwoFactorCache twoFactorCache)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _twoFactorCache = twoFactorCache;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(x => x.Email == request.Email))
            throw new InvalidOperationException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email
        };

        user.PasswordHash = _passwordHasher.Hash(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null || !_passwordHasher.Verify(user, request.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        if (user.TwoFactorEnabled)
        {
            var sessionId = Guid.NewGuid().ToString();

            await _twoFactorCache.SetAsync(
                sessionId,
                user.Id,
                TimeSpan.FromMinutes(5)
            );

            return new LoginResponse
            {
                RequiresTwoFactor = true,
                TwoFactorToken = sessionId
            };
        }

        return IssueTokens(user);
    }

    public async Task<LoginResponse> VerifyTwoFactorAsync(VerifyTwoFactorRequest request)
    {
        var userId = await _twoFactorCache.GetAsync(request.TwoFactorToken);
        if (userId == null)
            throw new UnauthorizedAccessException("2FA session expired");

        var user = await _db.Users.FindAsync(userId.Value);
        if (user == null)
            throw new UnauthorizedAccessException();

        await _twoFactorCache.RemoveAsync(request.TwoFactorToken);

        return IssueTokens(user);
    }

    public async Task<LoginResponse> RecoveryAsync(RecoveryCodeRequest request)
    {
        // TODO: verify recovery code thật
        throw new NotImplementedException();
    }

    private LoginResponse IssueTokens(User user)
    {
        return new LoginResponse
        {
            RequiresTwoFactor = false,
            AccessToken = _tokenService.GenerateAccessToken(user.Id, user.Email),
            RefreshToken = _tokenService.GenerateRefreshToken()
        };
    }
}
