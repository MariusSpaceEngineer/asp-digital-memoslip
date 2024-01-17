using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Consigner;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Application.Interfaces.Commands;
using AspDigitalMemoSlip.Application.Interfaces.Services;
using AspDigitalMemoSlip.Application.Services.Authentication;
using AspDigitalMemoSlip.Application.Utils;
using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;


namespace AspDigitalMemoSlip.Application.CQRS.Authentication
{
    public class RegisterCommand : IRequest<AuthResult>
    {
        public string ConsignerUserName { get; set; }
        public RegisterDTO Dto { get; set; }
        public IList<string> Roles { get; set; }

        public RegisterCommand(string consignerName, RegisterDTO model, IList<string> roles)
        {
            ConsignerUserName = consignerName;
            Dto = model;
            Roles = roles;
        }
    }

    public class RegisterCommandHandler : RegisterService, IRegisterCommand, IRequestHandler<RegisterCommand, AuthResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IEmailService _emailService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, HttpClient httpClient, IEmailService emailService) : base(userManager, roleManager)
        {
            _uow = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _httpClient = httpClient;
            _emailService = emailService;
        }


        public async Task<AuthResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            if (await DoesUserExistAsync(command.Dto.Username, command.Dto.Email))
                throw new UserAlreadyExistsException("User already exists");

            //Validates the VAT number filled by the client
            if (!await ValidateVatNumberAsync(command.Dto.VATNumber))
                throw new InvalidVatNumberException("Invalid VAT number");

            var user = await CreateUserAsync(command);

            // Send email to the newely created user notifying him that the account has been created and awaits further approvement by a consigner.
            await SendWelcomeEmail(user.Email, user.Name);

            return new AuthResult(StatusCodes.Status201Created, "Registration successful");
        }

        private async Task<bool> ValidateVatNumberAsync(string vatNumber)
        {
            var url = $"https://controleerbtwnummer.eu/api/validate/{vatNumber}.json";
            try
            {
                var vatValidationResult = await _httpClient.GetFromJsonAsync<VatValidationResult>(url);
                return vatValidationResult.Valid;
            }
            catch (HttpRequestException ex)
            {
                throw new VatValidationFailedExpection("VAT validation failed due to an HTTP request error", ex);
            }
            catch (JsonException ex)
            {
                throw new VatValidationFailedExpection("VAT validation failed due to a JSON parsing error", ex);
            }
        }


        private async Task<Domain.Consigner> GetAndValidateConsigner(string consignerUserName)
        {
            //sending the name in upper to make sure that is found by the usermanager
            var consigner = await _uow.ConsignerRepository.GetByUserName(consignerUserName.ToUpper());

            if (consigner == null)
            {
                throw new ConsignerNotFoundException("Consigner not found! Please check the consigner name and try again.");
            }

            return consigner;
        }


        public async Task<User> CreateUserAsync(RegisterCommand command)
        {
            //Making sure the consigner exists otherwise it shouldn't be possible to make a consignee
            var consigner = await GetAndValidateConsigner(command.ConsignerUserName);

            command.Dto.ConsignerId = consigner.Id;
            var user = _mapper.Map<Consignee>(command.Dto);

            //The client sends two pictures: idCopy and selfie which have to be worked on
            await ProcessUserImagesAsync((Consignee)user, command);

            var createUserResult = await _userManager.CreateAsync(user, command.Dto.Password);
            if (!createUserResult.Succeeded)
            {
                throw new UserCreationFailedException("User creation failed! Please check user details and try again.");
            }

            await AssignRolesAsync(user, command.Roles);
            user.SecurityStamp = Guid.NewGuid().ToString();
            return user;
        }

        private async Task ProcessUserImagesAsync(Consignee user, RegisterCommand command)
        {
            // Convert the images to byte arrays
            byte[] idImageBytes = ConvertToByteArray(command.Dto.IDCopy);
            byte[] selfieImageByte = ConvertToByteArray(command.Dto.Selfie);

            // Retrieve the key and IV from the environment variables
            byte[] key = Convert.FromBase64String(_configuration["AESKeys:Key"]);
            byte[] iv = Convert.FromBase64String(_configuration["AESKeys:IV"]);

            // Encrypt the images
            byte[] encryptedIdImage = AesEncryption.Encrypt(idImageBytes, key, iv);
            byte[] encryptedSelfieImage = AesEncryption.Encrypt(selfieImageByte, key, iv);

            // Save the encrypted images
            user.NationalRegistryCopyPath = await SaveEncryptedImageAsync(user.Id, command.Dto.IDCopy.FileName, encryptedIdImage, "IDCopy");
            user.SelfiePath = await SaveEncryptedImageAsync(user.Id, command.Dto.Selfie.FileName, encryptedSelfieImage, "Selfie");
        }


        private byte[] ConvertToByteArray(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Organizes the user's images into a directory structure. 
        /// Each user has a dedicated folder named after their userId. 
        /// Within each user's folder, images are further categorized into separate folders based on their type.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileName"></param>
        /// <param name="encryptedImage"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private async Task<string> SaveEncryptedImageAsync(string userId, string fileName, byte[] encryptedImage, string fileType)
        {
            string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string imagePath = Path.Combine(wwwrootPath, "Images", userId, fileType);
            Directory.CreateDirectory(imagePath);
            using (var stream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
            {
                await stream.WriteAsync(encryptedImage);
            }
            return Path.Combine("Images", userId, fileType, fileName);
        }

        private async Task SendWelcomeEmail(string to, string recipientName)
        {
            string subject = "Welcome to our platform!";
            string body = "Congratulations on creating your account! You will be notified when your account has been approved.";
            await _emailService.SendEmailAsync(to, recipientName, subject, body);
        }
    }
}
