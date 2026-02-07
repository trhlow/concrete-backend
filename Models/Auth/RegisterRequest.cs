namespace Concrete.Api.Models.Auth;

public class RegisterRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
