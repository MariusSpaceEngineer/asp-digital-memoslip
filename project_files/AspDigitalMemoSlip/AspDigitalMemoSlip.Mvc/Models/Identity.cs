namespace AspDigitalMemoSlip.Mvc.Models
{
    public class Identity
    {
        public int Id { get; set; }
        public int ConsigneeId { get; set; }
        public bool Approved { get; set; }
        public byte[]? Picture { get; set; }
    }
}
