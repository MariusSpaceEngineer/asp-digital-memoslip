using Microsoft.AspNetCore.Http;

namespace DTOClassLibrary.DTO.Authentication
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string VATNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string InsuranceCoverage { get; set; }
        public string NationalRegistryNumber { get; set; }
        public string NationalRegistryExpirationDate { get; set; }
        public string? ConsignerId { get; set; }


        public IFormFile Selfie { get; set; }
        public IFormFile IDCopy { get; set; }
    }
}
