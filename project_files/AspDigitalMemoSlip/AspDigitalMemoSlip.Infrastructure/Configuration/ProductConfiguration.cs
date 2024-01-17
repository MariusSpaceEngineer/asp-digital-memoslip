using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("tblProduct", "Product")
                   .HasKey(c => c.Id);

            builder.HasIndex(c => c.Id)
                   .IsUnique();

            builder.Property(c => c.Id)
                   .HasColumnType("int");

            builder.Property(c => c.MemoId)
                   .HasColumnType("int");

            builder.HasOne(p => p.Memo)
                   .WithMany(m => m.Products)
                   .HasForeignKey(p => p.MemoId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.SalesConfirmationId)
                   .HasColumnType("int")
                   .IsRequired(false);

            //replaced by productsale
            //builder.HasOne(p => p.SalesConfirmation)
            //       .WithMany(s => s.SoldProducts)
            //       .HasForeignKey(p => p.SalesConfirmationId)
            //       .IsRequired(false)
            //       .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.ConsignerId)
                   .IsRequired();

            builder.HasOne(p => p.Consigner)
                   .WithMany()
                   .HasForeignKey(p => p.ConsignerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.ConsigneeId)
                   .IsRequired();

            builder.HasOne(p => p.Consignee)
                   .WithMany()
                   .HasForeignKey(p => p.ConsigneeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.LotNumber)
                   .HasColumnType("nvarchar(30)");

            builder.Property(c => c.Description)
                   .HasColumnType("nvarchar(100)");

            builder.Property(c => c.Carat)
                   .HasColumnType("int");

            builder.Property(c => c.Price)
                   .HasColumnType("int");

            builder.Property(c => c.State)
                   .HasColumnType("int");

            builder.Property(c => c.Remarks)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false);

            //  builder.Property(c => c.CommissionPaidBy)
            //     .HasColumnType("bit");

            builder.Property(c => c.SuggestedPrice)
                   .HasColumnType("int");

            builder.Property(c => c.ProductSoldStatus)
                   .HasColumnType("int")
                   .HasConversion<int>();
        }
    }
}
