namespace AspDigitalMemoSlip.Mvc.Models
{
    public class ImageModel
    {
        public string FileContents { get; set; }
    }

    public class ConsigneeImagesModel
    {
        public ImageModel IdCopy { get; set; }
        public ImageModel Selfie { get; set; }
    }

}
