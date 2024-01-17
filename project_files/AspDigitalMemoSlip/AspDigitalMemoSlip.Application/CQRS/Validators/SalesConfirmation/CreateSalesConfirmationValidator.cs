using AspDigitalMemoSlip.Application.Commands;
using AspDigitalMemoSlip.Application.Interfaces;
using DTOClassLibrary.DTO.Product;
using FluentValidation;

namespace AspDigitalMemoSlip.Application.CQRS.Validators.SalesConfirmation
{
    public class CreateSalesConfirmationCommandValidator : AbstractValidator<CreateSalesConfirmationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSalesConfirmationCommandValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            RuleFor(s => s.ConsigneeId).NotEmpty().WithMessage("Consignee ID is required.");
            RuleFor(s => s.SalesConfirmation).NotNull().WithMessage("Sales Confirmation details are required.");
            RuleFor(s => s.SalesConfirmation.SoldProducts).NotEmpty().WithMessage("Can't create salesconfirmation without products");
            RuleForEach(s => s.SalesConfirmation.SoldProducts)
                .ChildRules(products =>
                {
                    products.RuleFor(p => p.CaratsSold).GreaterThanOrEqualTo(0).WithMessage("Selling less than 0 carats is not possible");
                });
            RuleFor(s => s.SalesConfirmation.SuggestedCommision)
            .InclusiveBetween(0, 100).WithMessage("Suggested commision needs to between 0 and 100");
            RuleForEach(s => s.SalesConfirmation.SoldProducts)
            .MustAsync(async (productSale, cancellation) =>
                await CheckCaratsOfProducts(productSale.ProductId, productSale.CaratsSold))
            .WithMessage("Cant sell more than there are in stock");

            RuleForEach(s => s.SalesConfirmation.SoldProducts)
            .MustAsync(async (productSale, cancellation) =>
                await CheckProductExist(productSale.ProductId))
            .WithMessage("No product exists with that id");

            RuleForEach(s => s.SalesConfirmation.SoldProducts)
                .MustAsync(async (productSale, cancellation) =>
                    await CheckProductState(productSale.ProductId))
                .WithMessage("Product cannot be in LOST, STOLEN, or RETURNED state.");
        }


        private async Task<bool> CheckCaratsOfProducts(int productId, int sellingCarats)
        {
            var product = await this._unitOfWork.ProductRepository.GetProductById(productId);
            return product.Carat >= sellingCarats;
        }

        private async Task<bool> CheckProductExist(int productId)
        {
            var product = await this._unitOfWork.ProductRepository.GetProductById(productId);
            return product != null;
        }

        private async Task<bool> CheckProductState(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(productId);

            return product.State != ProductState.LOST &&
                   product.State != ProductState.STOLEN &&
                   product.State != ProductState.RETURNED;
        }
    }
}
