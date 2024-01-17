using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Seeding
{
    public static class InvoiceSeeding
    {
        public static void Seed(this EntityTypeBuilder<Invoice> builder)
        {
            builder.HasData(new Invoice
            {
                Id = 1,
                Date = DateTime.Now,
                Total = 100.00,
                Commision = 10.00,
                CommisionStatus = Status.Paid,
                ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf7",
                ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6",


            }
            );
        }
    }
}
