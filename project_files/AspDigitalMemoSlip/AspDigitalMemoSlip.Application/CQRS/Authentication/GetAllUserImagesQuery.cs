using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Utils;
using AspDigitalMemoSlip.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AspDigitalMemoSlip.Application.CQRS.Authentication
{
    public class GetAllUserImagesQuery : IRequest<UserImagesResult>
    {
        public string UserId { get; set; }

        public GetAllUserImagesQuery(string userId)
        {
            UserId = userId;

        }
    }
    public class GetImageQueryHandler : IRequestHandler<GetAllUserImagesQuery, UserImagesResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public GetImageQueryHandler(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserImagesResult> Handle(GetAllUserImagesQuery query, CancellationToken cancellationToken)
        {
            // Get the image paths
            var user = await GetUser(query.UserId);
            string idCopyImagePath = GetImagePath(user.NationalRegistryCopyPath);
            string selfieImagePath = GetImagePath(user.SelfiePath);

            // Decrypt the images
            byte[] decryptedIdCopyImageBytes = DecryptImage(idCopyImagePath);
            byte[] decryptedSelfieBytes = DecryptImage(selfieImagePath);

            // Create a dictionary to store both images
            var images = new Dictionary<string, byte[]>
            {
                { "IDCopy", decryptedIdCopyImageBytes },
                { "Selfie", decryptedSelfieBytes }
            };

            // Convert the images in the dictionary to FileContentResult objects
            var fileContentResults = ConvertToContentResult(images);

            // Return the FileContentResult objects
            return new UserImagesResult
            {
                IDCopy = fileContentResults["IDCopy"],
                Selfie = fileContentResults["Selfie"]
            };
        }

        private async Task<Consignee> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) as Consignee;
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }
            return user;
        }

        private string GetImagePath(string path)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"File not found.");
            }
            return imagePath;
        }

        private byte[] DecryptImage(string imagePath)
        {
            byte[] encryptedImage = File.ReadAllBytes(imagePath);

            // Retrieve the key and IV from the environment variables
            byte[] key = Convert.FromBase64String(_configuration["AESKeys:Key"]);
            byte[] iv = Convert.FromBase64String(_configuration["AESKeys:IV"]);

            return AesEncryption.Decrypt(encryptedImage, key, iv);
        }

        private Dictionary<string, FileContentResult> ConvertToContentResult(Dictionary<string, byte[]> images)
        {
            var fileContentResults = new Dictionary<string, FileContentResult>();
            foreach (var image in images)
            {
                var fileContentResult = new FileContentResult(image.Value, "image/jpeg")
                {
                    FileDownloadName = $"{image.Key}.jpg"
                };
                fileContentResults.Add(image.Key, fileContentResult);
            }
            return fileContentResults;
        }


    }
}
