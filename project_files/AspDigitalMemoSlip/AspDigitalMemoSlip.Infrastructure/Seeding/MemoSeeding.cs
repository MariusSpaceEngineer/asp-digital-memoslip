using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Seeding
{
    public static class MemoSeeding
    {
        public static void Seed(this EntityTypeBuilder<Memo> builder)
        {
            builder.HasData(new Memo
            {
                Id = 1,
                TermsAndConditionsId = 1,
                TermsAccepted = true,
                ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                CreateDate = DateTime.Now,
                AcceptedByConsignee = false,
            },
            new Memo // Remove the closing parenthesis here
            {
                Id = 2,
                TermsAndConditionsId = 1,
                TermsAccepted = true,
                ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6", // Replace with actual ConsigneeId
                ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf7", // Replace with actual ConsignerId
                CreateDate = DateTime.Now,
                AcceptedByConsignee = false,
            });
        }
    }
}
