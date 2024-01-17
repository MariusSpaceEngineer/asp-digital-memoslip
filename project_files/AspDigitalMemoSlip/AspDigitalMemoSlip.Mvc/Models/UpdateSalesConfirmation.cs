namespace AspDigitalMemoSlip.Mvc.Models
{
    public class SalesConfirmationUpdateModel
    {
        public int SalesConfirmationId { get; set; }
        public double? Commision { get; set; }
        public Dictionary<int, ProductDecision> UpdatedProductsSale { get; set; }
    }

    public class ProductDecision
    {
        public string Decision { get; set; }
        public string Price { get; set; }
    }
}
