using System.Security.Claims;

namespace Concrete.Api.Helpers
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId!);
        }
    }
}
