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
    public class MemoConfiguration : IEntityTypeConfiguration<Memo>
    {
        public void Configure(EntityTypeBuilder<Memo> builder)
        {
            builder.ToTable("tblMemo", "Memo")
                   .HasKey(c => c.Id);

            builder.HasIndex(c => c.Id)
                .IsUnique();

            builder.Property(c => c.Id)
                .HasColumnType("int");

            builder.Property(c => c.CreateDate)
                .HasColumnType("datetime")  
                .IsRequired();

            builder.HasMany(m => m.Products)
              .WithOne(p => p.Memo)
              .HasForeignKey(p => p.MemoId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Consigner)
               .WithMany(c => c.Memos)
               .HasForeignKey(m => m.ConsignerId)
               .IsRequired();

            builder.HasOne(m => m.Consignee)
                .WithMany(c => c.Memos)
                .HasForeignKey(m => m.ConsigneeId)
                .IsRequired();

        }
    }
}
