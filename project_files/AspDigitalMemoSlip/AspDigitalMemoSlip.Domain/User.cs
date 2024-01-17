using Microsoft.AspNetCore.Identity;

namespace AspDigitalMemoSlip.Domain
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string? TwoFactorSecretKey { get; set; }

    }
}
