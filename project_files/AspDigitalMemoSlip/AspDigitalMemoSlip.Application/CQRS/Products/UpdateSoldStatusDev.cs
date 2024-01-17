using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using MediatR;
using static AspDigitalMemoSlip.Domain.Product;

namespace AspDigitalMemoSlip.Application.CQRS.Products
{

    public class UpdateSoldStatusDev : IRequest<Product>
    {
        public int ProductId { get; set; }
        public SoldStatus Status { get; set; }

    }

    public class UpdateSoldStatusDevHandler : IRequestHandler<UpdateSoldStatusDev, Product>
    {
        private readonly IUnitOfWork uow;

        public UpdateSoldStatusDevHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Product> Handle(UpdateSoldStatusDev request, CancellationToken cancellationToken)
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

            uow.ProductRepository.Update(existingProduct);
            await uow.Commit();

            return existingProduct;
        }
    }
}

