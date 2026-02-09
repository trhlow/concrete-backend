namespace Concrete.Api.DTOs.Auth;

public class LoginResponse
{
    public bool RequiresTwoFactor { get; set; }

    public string? TwoFactorToken { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }
}
