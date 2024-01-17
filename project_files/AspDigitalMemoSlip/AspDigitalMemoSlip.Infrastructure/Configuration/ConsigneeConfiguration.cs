using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Configuration
{
    public class ConsigneeConfiguration : IEntityTypeConfiguration<Consignee>
	{
        public void Configure(EntityTypeBuilder<Consignee> builder)
        {
            builder.ToTable("tblConsignee", "Consignee");

            builder.Property(c => c.Id)
            .IsRequired();

            builder.Property(c => c.VATNumber)
                .HasColumnType("nvarchar(20)")
                .IsRequired();

            builder.Property(c => c.InsuranceNumber)
                .HasColumnType("nvarchar(20)");

            builder.Property(c => c.InsuranceCoverage)
                .HasColumnType("float");

            builder.Property(c => c.SelfiePath)
                .HasColumnType("nvarchar(max)");

            builder.Property(c => c.NationalRegistryCopyPath)
                .HasColumnType("nvarchar(max)");

            builder.Property(c => c.NationalRegistryNumber)
                .HasColumnType("nvarchar(11)")
                .IsRequired();

            builder.Property(c => c.TwoFactorSecretKey)
                .HasColumnType("nvarchar(max)");

            builder.Property(c => c.NationalRegistryExpirationDate)
                .HasColumnType("Date");

            builder.Property(c => c.AcceptedByConsigner)
                .HasColumnType("bit")
                .HasDefaultValue(false);

            builder.HasMany(c => c.Memos)
                .WithOne(m => m.Consignee)
                .HasForeignKey(m => m.ConsigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Consigner)
               .WithMany(c => c.Consignees)
               .HasForeignKey(c => c.ConsignerId)
               .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
