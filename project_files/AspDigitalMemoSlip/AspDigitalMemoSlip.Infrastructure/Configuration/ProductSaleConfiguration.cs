using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspDigitalMemoSlip.Domain;

namespace AspDigitalMemoSlip.Infrastructure.Configuration
{
    internal class ProductSaleConfiguration : IEntityTypeConfiguration<ProductSale>
    {
        public void Configure(EntityTypeBuilder<ProductSale> builder)
        {
            builder.ToTable("tblProductSale", "ProductSale")
               .HasKey(c => c.Id);

            builder.HasIndex(c => c.Id)
                   .IsUnique();

            builder.Property(c => c.Id)
                   .HasColumnType("int");

            builder.HasOne(s => s.SalesConfirmation)
               .WithMany()
               .HasForeignKey(s => s.SalesConfirmationId)
               .IsRequired();
        }
    }
}
