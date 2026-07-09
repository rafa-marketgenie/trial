using System.Security.Claims;

namespace trial.Utils
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out int id))
            {
                return id;
            }
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }
    }
}