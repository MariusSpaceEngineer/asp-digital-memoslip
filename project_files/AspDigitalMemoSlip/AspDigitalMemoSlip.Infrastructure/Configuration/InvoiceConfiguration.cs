using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            // Primary key
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Total).IsRequired();
            builder.Property(e => e.Commision).IsRequired();
            builder.Property(e => e.CommisionStatus).IsRequired();
            builder.Property(e => e.ConsigneeId).IsRequired();
            builder.Property(e => e.ConsignerId).IsRequired();

            // Relationships
            builder.HasOne(e => e.Consignee)
                .WithMany()
                .HasForeignKey(e => e.ConsigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Consigner)
                .WithMany()
                .HasForeignKey(e => e.ConsignerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enumeration mapping
            builder.Property(e => e.CommisionStatus)
                .HasConversion<string>();

            // Navigation property for SaleConfirmation
            builder.HasMany(e => e.SaleConfirmation)
                .WithOne(sc => sc.Invoice)
                .HasForeignKey(sc => sc.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
