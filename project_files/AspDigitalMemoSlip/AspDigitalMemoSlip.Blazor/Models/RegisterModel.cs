using Microsoft.AspNetCore.Components.Forms;

namespace AspDigitalMemoSlip.Blazor.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string VATNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public bool TCPlatform { get; set; }
        public bool TCMemo { get; set; }
        public decimal InsuranceCoverage { get; set; }
        public string NationalRegistryNumber { get; set; }
        public DateTime NationalRegistryExpirationDate { get; set; }
        public IBrowserFile IDCopy { get; set; }
        public IBrowserFile RegistrationProof { get; set; }
        public IBrowserFile Selfie { get; set; }

    }
}
