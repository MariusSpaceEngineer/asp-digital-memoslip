namespace AspDigitalMemoSlip.Domain
{
    public class TermsAndCondition
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<TermsAndCondition> TermsAndCondisions { get; set; }
    }
}
