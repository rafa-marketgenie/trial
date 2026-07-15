using System.Security.Claims;

namespace trial.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(idClaim, out Guid id))
            {
                return id;
            }
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }
    }
}