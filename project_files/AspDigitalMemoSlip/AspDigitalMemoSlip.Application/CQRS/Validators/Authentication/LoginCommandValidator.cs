using AspDigitalMemoSlip.Application.CQRS.Authentication;
using FluentValidation;

namespace AspDigitalMemoSlip.Application.CQRS.Validators.Authentication
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Dto.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Username can only contain alphanumeric characters.");

            RuleFor(x => x.Dto.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one digit.")
                .Matches(@"\W+").WithMessage("Password must contain at least one non-alphanumeric character.")
                .Matches(@"[\W_]+").WithMessage("Password must contain at least one special character.")
                .Unless(x => string.IsNullOrEmpty(x.Dto.Password));

            RuleFor(x => x.Dto.OTCode)
                .Length(6).WithMessage("Multi-Factor code must be 6 digits long.")
                .Matches(@"^[0-9]*$").WithMessage("Multi-Factor code can only contain digits.")
                .Unless(x => string.IsNullOrEmpty(x.Dto.OTCode));

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url is required.");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Dto.Password) || !string.IsNullOrEmpty(x.Dto.OTCode))
                .WithMessage("Either Password or OTCode must be provided.");

        }
    }


}
