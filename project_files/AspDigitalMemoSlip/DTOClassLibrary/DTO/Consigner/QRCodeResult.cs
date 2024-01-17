namespace DTOClassLibrary.DTO.Consigner
{
    public class QRCodeResult
    {
        public string Url { get; set; }
        public string QRcode { get; set; }

        public QRCodeResult(string url, string qrCode)
        {
            Url = url;
            QRcode = qrCode;
        }
    }
}
