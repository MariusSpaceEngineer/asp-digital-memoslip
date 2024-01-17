using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using MediatR;
using static AspDigitalMemoSlip.Domain.Product;

namespace AspDigitalMemoSlip.Application.CQRS.Products
{
    public class UpdateSoldStatusCounter : IRequest<Product>
    {
        public int ProductId { get; set; }
        public SoldStatus Status { get; set; }
        public int SuggestedPrice { get; set; }
        public double CommisionPrice { get; set; }
        public int CommissionPaidBy { get; set; }
    }

    public class UpdateSoldStatusCounterHandler : IRequestHandler<UpdateSoldStatusCounter, Product>
    {
        private readonly IUnitOfWork uow;

        public UpdateSoldStatusCounterHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Product> Handle(UpdateSoldStatusCounter request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The provided command object is null.");
            }

            // Retrieve the existing product from the database
            var existingProduct = await uow.ProductRepository.GetById(request.ProductId);

            if (existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {request.ProductId} not found.", nameof(request.ProductId));
            }

            // Update the product status
            existingProduct.ProductSoldStatus = request.Status;
            // Update the Commision Price
            existingProduct.CommisionPrice = request.CommisionPrice;
            // Update the suggested price
            existingProduct.SuggestedPrice = request.SuggestedPrice;
            // Update commission paid by 
            //  existingProduct.CommissionPaidBy = request.CommissionPaidBy;

            uow.ProductRepository.Update(existingProduct);
            await uow.Commit();

            return existingProduct;
        }
    }
}
