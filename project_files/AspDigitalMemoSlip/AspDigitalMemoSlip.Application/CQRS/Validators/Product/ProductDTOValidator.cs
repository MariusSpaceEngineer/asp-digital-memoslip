using FluentValidation;
using DTOClassLibrary.DTO.Product;

public class ProductDTOValidator : AbstractValidator<ProductDTO>
{
    public ProductDTOValidator()
    {
        RuleFor(product => product.ID)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(product => product.ConsignerId)
            .NotEmpty().WithMessage("Consigner ID is required.");

        RuleFor(product => product.ConsigneeId)
            .NotEmpty().WithMessage("Consignee ID is required.");

        RuleFor(product => product.LotNumber)
            .NotEmpty().WithMessage("Lot Number is required.");

        RuleFor(product => product.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description is too long.");

        RuleFor(product => product.Carat)
            .GreaterThan(0).WithMessage("Carat must be a positive number.");

        RuleFor(product => product.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative number.");

    }
}
