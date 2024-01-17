namespace AspDigitalMemoSlip.Domain
{
    public class Consigner : User
    {
        public ICollection<Memo> Memos { get; set; }
        public ICollection<Consignee> Consignees { get; set; }



        public Consigner() {
            Memos = new List<Memo>();
            Consignees = new List<Consignee>();
        }

    }
}
