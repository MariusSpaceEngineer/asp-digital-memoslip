using FluentValidation;
using DTOClassLibrary.DTO.Consigner;
using FluentValidation.Validators;
using MediatR;
using AspDigitalMemoSlip.Application.CQRS.Consigner;

public class ConsignerDTOValidator : AbstractValidator<ConsignerDTO>
{

    private readonly IMediator _mediator;

    public ConsignerDTOValidator(IMediator mediator)
    {
        _mediator = mediator;

        RuleFor(consigner => consigner.Id)
            .NotEmpty().WithMessage("Consigner ID is required.")
            .MustAsync(BeAValidConsigner)
            .WithMessage("Consigner ID does not exist in the database.");

    }
    private async Task<bool> BeAValidConsigner(string consignerId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new ConsignerExistsQuery(consignerId), cancellationToken);
    }

}
