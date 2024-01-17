using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Infrastructure.Configuration
{
    internal class SalesConfirmationConfiguration : IEntityTypeConfiguration<SalesConfirmation>
    {
        public void Configure(EntityTypeBuilder<SalesConfirmation> builder)
        {
            builder.ToTable("tblSalesConfirmation", "SalesConfirmation")
               .HasKey(c => c.Id);

            builder.HasIndex(c => c.Id)
                   .IsUnique();

            builder.Property(c => c.Id)
                   .HasColumnType("int");

            builder.HasOne(s => s.Consignee)
               .WithMany() 
               .HasForeignKey(s => s.ConsigneeId) 
               .IsRequired();

            builder.HasMany(s => s.ProductSales)
                   .WithOne(p => p.SalesConfirmation)
                   .HasForeignKey(p => p.SalesConfirmationId)
                   .IsRequired(false); 
        }
    }
}
