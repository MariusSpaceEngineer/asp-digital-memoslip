namespace AspDigitalMemoSlip.Blazor.Models
{
    public class UserImagesModel
    {
        public string FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileDownloadName { get; set; }
        public object LastModified { get; set; }
        public object EntityTag { get; set; }
        public bool EnableRangeProcessing { get; set; }
    }
}
