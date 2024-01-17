using AspDigitalMemoSlip.Application.Utils.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspDigitalMemoSlip.Infrastructure.Seeding
{
    public static class RolesSeeding
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "1", Name = UserRole.Consigner, NormalizedName = UserRole.Consigner.ToUpper() },
             new IdentityRole { Id = "2", Name = UserRole.Consignee, NormalizedName = UserRole.Consignee.ToUpper() },
             new IdentityRole { Id = "3", Name = UserRole.Admin, NormalizedName = UserRole.Admin.ToUpper() }
           );

        }
    }
}
