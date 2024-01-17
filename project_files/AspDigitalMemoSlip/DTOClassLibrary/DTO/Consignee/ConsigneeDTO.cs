namespace DTOClassLibrary.DTO.Consignee
{
    public class ConsigneeDTO
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? InsuranceNumber { get; set; }
        public double? InsuranceCoverage { get; set; }
        public string? NationalRegistryNumber { get; set; }
        public DateTime? NationalRegistryExpirationDate { get; set; }
        public string? NationalRegistryCopyPath { get; set; }
        public string? VATNumber { get; set; }
        public string? SelfiePath { get; set; }
        public bool? AcceptedByConsignee { get; set; }


    }
}
