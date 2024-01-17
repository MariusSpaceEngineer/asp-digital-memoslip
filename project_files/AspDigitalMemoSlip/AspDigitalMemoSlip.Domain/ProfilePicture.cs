namespace AspDigitalMemoSlip.Domain
{
    public class ProfilePicture
    {
        public int Id { get; set; }
        public int ConsigneeId { get; set; }
        public bool Approved { get; set; }
        public byte[]? Picture { get; set; }
    }
}
