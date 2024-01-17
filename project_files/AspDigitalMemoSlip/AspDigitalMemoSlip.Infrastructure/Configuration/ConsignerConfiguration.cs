using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Configuration
{
    public class ConsignerConfiguration : IEntityTypeConfiguration<Consigner>
    {
        public void Configure(EntityTypeBuilder<Consigner> builder)
        {
            builder.ToTable("tblConsigner", "Consigner");

            builder.Property(c => c.Id)
           .IsRequired();

            builder.HasMany(c => c.Memos)
                .WithOne(m => m.Consigner)
                .HasForeignKey(m => m.ConsignerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Consignees)
                .WithOne(c => c.Consigner)
                .HasForeignKey(c => c.ConsignerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
