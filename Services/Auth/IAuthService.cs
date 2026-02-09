using Concrete.Api.DTOs.Auth;

namespace Concrete.Api.Services.Auth;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> VerifyTwoFactorAsync(VerifyTwoFactorRequest request);
    Task<LoginResponse> RecoveryAsync(RecoveryCodeRequest request);
}
