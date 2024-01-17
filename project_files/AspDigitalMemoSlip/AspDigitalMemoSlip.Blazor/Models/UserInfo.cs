namespace AspDigitalMemoSlip.Blazor.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string VatNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string InsuranceCoverage { get; set; }
        public string QrCode { get; set; }
        public string IdCopyImageBase64 { get; set; }
        public string SelfieImageBase64 { get; set; }
        public bool MfaEnabled { get; set; }
        public string IdNumber { get; set; }
        public string IdExpireDate { get; set; }
        public bool? IdWillExpire { get; set; }
    }

}
