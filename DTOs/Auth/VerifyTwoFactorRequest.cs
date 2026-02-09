using System.ComponentModel.DataAnnotations;

namespace Concrete.Api.DTOs.Auth;

public class VerifyTwoFactorRequest
{
    [Required]
    public string TwoFactorToken { get; set; } = null!;

    [Required]
    public string Code { get; set; } = null!;
}
