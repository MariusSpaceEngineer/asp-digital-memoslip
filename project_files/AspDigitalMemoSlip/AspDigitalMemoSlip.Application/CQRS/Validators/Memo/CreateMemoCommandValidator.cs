using FluentValidation;
using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Product;
using MediatR;
using AspDigitalMemoSlip.Application.CQRS.Validators.Consignee;

public class CreateMemoCommandValidator : AbstractValidator<CreateMemoCommand>
{
    private readonly IMediator _mediator;

    public CreateMemoCommandValidator(IMediator mediator)
    {
        _mediator = mediator;

        RuleFor(command => command.Memo.Products)
            .NotEmpty().WithMessage("Product list cannot be empty.")
            .ForEach(productRule =>
            {
                productRule.Must(product => product.Carat >= 0)
                    .WithMessage("Product carat value cannot be negative.");

                productRule.Must(product => product.Price >= 0)
                    .WithMessage("Product price cannot be negative.");
            });

        RuleFor(command => command.Memo.TermsAccepted)
            .Equal(true).WithMessage("Terms must be accepted.");

        RuleFor(command => command.Memo.ConsignerId)
            .NotEmpty().WithMessage("Consigner ID cannot be empty.");

        RuleFor(command => command.Memo.ConsigneeId)
            .NotEmpty().WithMessage("Consignee ID cannot be empty.");
    }
}

