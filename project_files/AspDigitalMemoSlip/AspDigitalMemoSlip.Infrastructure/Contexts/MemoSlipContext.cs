using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Configuration;
using AspDigitalMemoSlip.Infrastructure.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AspDigitalMemoSlip.Infrastructure.Contexts
{
    public class MemoSlipContext : IdentityDbContext<User>
    {


        public MemoSlipContext(DbContextOptions<MemoSlipContext> options) : base(options)
        {
        }

        public DbSet<Memo> Memos { get; set; }
        public DbSet<Consignee> Consignees { get; set; }
        public DbSet<Consigner> Consigners { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesConfirmation> SalesConfirmations { get; set; }

        //delete
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<GenericNotification> GenericNotifications { get; set; }
        public DbSet<ProductSale> ProductsSale { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new SalesConfirmationConfiguration());
            modelBuilder.ApplyConfiguration(new ConsigneeConfiguration());
            modelBuilder.ApplyConfiguration(new ConsignerConfiguration());
            modelBuilder.ApplyConfiguration(new MemoConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.SeedRoles();
            modelBuilder.SeedUsers();
            modelBuilder.Entity<Memo>().Seed();
            modelBuilder.Entity<Product>().Seed();
            modelBuilder.Entity<Invoice>().Seed();
            modelBuilder.Entity<SalesConfirmation>().Seed();

        }
    }
}
