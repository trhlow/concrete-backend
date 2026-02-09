using System.ComponentModel.DataAnnotations;

namespace Concrete.Api.DTOs.Auth;

public class RecoveryCodeRequest
{
    [Required]
    public string TwoFactorToken { get; set; } = null!;

    [Required]
    public string RecoveryCode { get; set; } = null!;
}
