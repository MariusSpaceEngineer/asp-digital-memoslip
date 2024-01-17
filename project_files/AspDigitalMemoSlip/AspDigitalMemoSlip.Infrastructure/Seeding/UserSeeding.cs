using AspDigitalMemoSlip.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspDigitalMemoSlip.Infrastructure.Seeding
{
    public static class UserSeeding
    {
        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedAdmin();
            modelBuilder.SeedConsignersAndConsignees();
        }

        public static void SeedAdmin(this ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<User>();

            var adminUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                EmailConfirmed = true,
                Name = "Admin",
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            modelBuilder.Entity<User>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = "3"
            });
        }

        public static void SeedConsignersAndConsignees(this ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<User>();

            var consignerUser = new Consigner
            {
                Id = "341743f0-asd2–42de-afbf-59kmkkmk72cf6", // Use the UserId as the primary key
                UserName = "DevJewels", // Use the UserName property from the User class
                NormalizedUserName = "DEVJEWELS",
                Name = "Consigner",
                PhoneNumber = "1234567890",
                Email = "consigner1@example.com",
                NormalizedEmail = "CONSIGNER1@EXAMPLE.COM",
                PasswordHash = hasher.HashPassword(null, "Password123!")
            };

            var consigneeUser = new Consignee
            {
                Id = "02174cf0–9412–4cfe-afbf-59f706d72cf6", // Use the UserId as the primary key
                UserName = "Consignee1", // Use the UserName property from the User class
                NormalizedUserName = "CONSIGNEE1",
                Name = "Thomas Derwaal",
                PhoneNumber = "0489567257",
                Email = "consignee1@example.com",
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                NationalRegistryNumber = "43545634596",
                NationalRegistryExpirationDate = DateTime.Now.AddYears(1),
                VATNumber = "BE0123456789",
                InsuranceNumber = "12345678910",
                InsuranceCoverage = 23.6,
                AcceptedByConsigner = true, // The user is not activated
                ConsignerId = consignerUser.Id, // Link the consignee to the consigner
            };

            var consigneeUser2 = new Consignee
            {
                Id = "02174cf0–9412–4cfe-afbf-59f706d72cf7", // Use a different UserId as the primary key
                UserName = "Consignee2", // Use a different UserName
                NormalizedUserName = "CONSIGNEE2",
                Name = "Jef Jefsensen",
                PhoneNumber = "0485526317",
                Email = "consignee2@example.com",
                PasswordHash = hasher.HashPassword(null, "Password123!"),
                NationalRegistryNumber = "48684235796",
                NationalRegistryExpirationDate = DateTime.Now.AddYears(1),
                VATNumber = "BE0123457895",
                InsuranceNumber = "12349878985",
                InsuranceCoverage = 23.6,
                AcceptedByConsigner = false, // The user is not activated
                ConsignerId = consignerUser.Id, // Link the consignee to the same consigner
            };


            modelBuilder.Entity<Consigner>().HasData(consignerUser);
            modelBuilder.Entity<Consignee>().HasData(consigneeUser);
            modelBuilder.Entity<Consignee>().HasData(consigneeUser2);


            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = consigneeUser.Id,
                RoleId = "2"
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = consigneeUser2.Id,
                RoleId = "2"
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = consignerUser.Id,
                RoleId = "1"
            });
        }
    }

}
