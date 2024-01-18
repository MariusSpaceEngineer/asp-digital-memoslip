using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Seeding
{
    public static class SalesConfirmationSeeding
    {
        public static void Seed(this EntityTypeBuilder<SalesConfirmation> builder)
        {
            builder.HasData(new SalesConfirmation
            {
                Id = 1,
                ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf7",
                CreatedDate = DateTime.Now,
                InvoiceId = 1,
                SuggestedCommision = 4

            }
            );
        }
    }
}
