using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetEmail(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.Email);

        public static string? GetDistributorId(this ClaimsPrincipal principal)
            => principal.FindFirstValue("DistributorId");

        public static string? GetDistributorName(this ClaimsPrincipal principal)
            => principal.FindFirstValue("DistributorName");

        public static string? GetFullName(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue("FullName");
        }


        public static string? Role(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(ClaimTypes.Role)?.Value;
        }
        public static string? GetFirstName(this ClaimsPrincipal principal)
            => principal?.FindFirst(ClaimTypes.Name)?.Value;

        public static string? GetSurname(this ClaimsPrincipal principal)
            => principal?.FindFirst(ClaimTypes.Surname)?.Value;

        public static string? GetPhoneNumber(this ClaimsPrincipal principal)
            => principal.FindFirstValue(ClaimTypes.MobilePhone);

        public static string? GetUserId(this ClaimsPrincipal principal)
           => principal.FindFirstValue(ClaimTypes.NameIdentifier);

        private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
            principal is null
                ? throw new ArgumentNullException(nameof(principal))
                : principal.FindFirst(claimType)?.Value;
    }
}
