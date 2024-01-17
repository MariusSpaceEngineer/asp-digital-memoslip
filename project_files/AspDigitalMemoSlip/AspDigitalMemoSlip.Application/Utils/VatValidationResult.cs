namespace AspDigitalMemoSlip.Application.Utils
{
    public class VatValidationResult
    {
        public bool Valid { get; set; }
        public string CountryCode { get; set; }
        public string VatNumber { get; set; }
        public string? Name { get; set; }
        public Address Address { get; set; }
        public string? StrAddress { get; set; }
    }

    public class Address
    {
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string Country { get; set; }
    }

}
