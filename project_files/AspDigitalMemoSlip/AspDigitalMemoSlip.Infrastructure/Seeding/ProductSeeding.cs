using AspDigitalMemoSlip.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspDigitalMemoSlip.Infrastructure.Seeding
{
    public static class ProductSeeding
    {
        public static void Seed(this EntityTypeBuilder<Product> builder)
        {

            builder.HasData(new Product
            {
                Id = 1,
                MemoId = 1,
                Carat = 18,
                Description = "diamant",
                LotNumber = "132454",
                Price = 140,
                ProductSoldStatus = Product.SoldStatus.NotSold,
                SuggestedPrice = 132,
                CommisionPrice = 0.1,
                SalesConfirmationId = null,
                ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6"
            },
                new Product
                {
                    Id = 2,
                    MemoId = 1,
                    Carat = 18,
                    Description = "diamant",
                    LotNumber = "132454",
                    Price = 140,
                    ProductSoldStatus = Product.SoldStatus.NotSold,
                    SuggestedPrice = 132,
                    CommisionPrice = 0.2,
                    SalesConfirmationId = null,
                    ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                    ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6"
                },
                new Product
                {
                    Id = 3,
                    MemoId = 1,
                    Carat = 18,
                    Description = "diamant",
                    LotNumber = "132454",
                    Price = 140,
                    ProductSoldStatus = Product.SoldStatus.NotSold,
                    SuggestedPrice = 132,
                    CommisionPrice = 0.3,
                    SalesConfirmationId = null,
                    ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                    ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6"
                },
                new Product
                {
                    Id = 4,
                    MemoId = 2, // Set MemoId to 2 for this record
                    Carat = 18,
                    Description = "diamant",
                    LotNumber = "132454",
                    Price = 140,
                    ProductSoldStatus = Product.SoldStatus.NotSold,
                    SuggestedPrice = 132,
                    CommisionPrice = 0.1,
                    SalesConfirmationId = null,
                    ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6",
                    ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6"
                },
                new Product
                {
                    Id = 5,
                    MemoId = 2, // Set MemoId to 2 for this record
                    Carat = 18,
                    Description = "diamant",
                    LotNumber = "132454",
                    Price = 140,
                    ProductSoldStatus = Product.SoldStatus.NotSold,
                    SuggestedPrice = 132,
                    CommisionPrice = 0.2,
                    SalesConfirmationId = null,
                    ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                    ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6"
                },
                 new Product
                 {
                     Id = 6,
                     MemoId = 2, // Set MemoId to 2 for this record
                     Carat = 18,
                     Description = "diamant",
                     LotNumber = "132454",
                     Price = 140,
                     ProductSoldStatus = Product.SoldStatus.NotSold,
                     SuggestedPrice = 132,
                     CommisionPrice = 0.3,
                     SalesConfirmationId = null,
                     ConsigneeId = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                     ConsignerId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6"
                 }
                         );
        }

    }
}
