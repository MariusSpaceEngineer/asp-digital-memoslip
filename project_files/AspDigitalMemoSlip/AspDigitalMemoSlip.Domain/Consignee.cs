namespace AspDigitalMemoSlip.Domain
{
    public class Consignee : User
    {
        public string VATNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public double InsuranceCoverage { get; set; }
        public string? SelfiePath { get; set; }
        public string? NationalRegistryCopyPath { get; set; }
        public string NationalRegistryNumber { get; set; }
        public DateTime? NationalRegistryExpirationDate { get; set; }
        public ICollection<Memo> Memos { get; set; }
        public bool AcceptedByConsigner { get; set; }
        public string ConsignerId { get; set; }
        public Consigner Consigner { get; set; }



        public Consignee()
        {
            Memos = new List<Memo>();
        }
    }

}
