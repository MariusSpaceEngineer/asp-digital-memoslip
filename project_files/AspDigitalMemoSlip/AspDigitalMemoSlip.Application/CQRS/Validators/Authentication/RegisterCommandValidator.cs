namespace AspDigitalMemoSlip.Application.CQRS.Validators.Authentication
{
    using AspDigitalMemoSlip.Application.CQRS.Authentication;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using System.Globalization;

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.ConsignerUserName)
                .NotEmpty().WithMessage("Consigner username is required.")
                .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Consigner username can only contain alphanumeric characters.");

            RuleFor(x => x.Dto.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^[a-zA-Z0-9]*$").WithMessage("Username can only contain alphanumeric characters.");

            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one digit.")
                .Matches(@"\W+").WithMessage("Password must contain at least one non-alphanumeric character.");

            RuleFor(x => x.Dto.PhoneNumber)
               .NotEmpty().WithMessage("Phone number is required.")
               .Matches(@"^\+?\d+$").WithMessage("Phone number can only contain digits and optionally a '+' prefix.")
               .Length(4, 16).WithMessage("Phone number must be between 4 and 16 characters long.");

            RuleFor(x => x.Dto.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Dto.VATNumber)
                .NotEmpty().WithMessage("VAT number is required.");

            RuleFor(x => x.Dto.InsuranceNumber)
             .NotEmpty().WithMessage("Insurance coverage is required.");

            RuleFor(x => x.Dto.InsuranceCoverage)
               .NotEmpty().WithMessage("Insurance coverage is required.")
               .Matches(@"^\d+([.,]\d+)?$").WithMessage("Insurance coverage can only contain numbers and optionally a decimal point or comma.");

            RuleFor(x => x.Dto.NationalRegistryNumber)
                .NotEmpty().WithMessage("National registry number is required.");

            RuleFor(x => x.Dto.NationalRegistryExpirationDate)
                .NotEmpty().WithMessage("National registry expirationDate is required.")
                .Must(BeAValidDate).WithMessage("National registry expirationDate must be a valid date.");

            RuleFor(x => x.Dto.Selfie)
                .NotNull().WithMessage("Selfie is required.")
                .Must(BeAValidImage).WithMessage("Selfie must be a valid image file.");

            RuleFor(x => x.Dto.IDCopy)
                .NotNull().WithMessage("ID-Copy is required.")
                .Must(BeAValidImageOrPdf).WithMessage("ID-Copy must be a valid image file.");

            RuleFor(x => x.Roles)
                .NotEmpty().WithMessage("Roles are required.");
        }
        private bool BeAValidDate(string dateString)
        {
            var allowedFormats = new[] { "d-M-yyyy", "M/d/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "MM-dd-yyyy", "M-d-yyyy", "yyyy/M/d" };
            return DateTime.TryParseExact(dateString, allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool BeAValidImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".tiff" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension);
        }

        private bool BeAValidImageOrPdf(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension);
        }
    }

}
