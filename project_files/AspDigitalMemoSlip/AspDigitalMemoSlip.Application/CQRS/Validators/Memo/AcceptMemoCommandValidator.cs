using FluentValidation;
using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using DTOClassLibrary.DTO.Memo;

public class AcceptMemoCommandValidator : AbstractValidator<AcceptMemoCommand>
{
    public AcceptMemoCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(command => command.AcceptMemoDTO)
            .NotNull().WithMessage("Accept memo data is required.");
    }
}
