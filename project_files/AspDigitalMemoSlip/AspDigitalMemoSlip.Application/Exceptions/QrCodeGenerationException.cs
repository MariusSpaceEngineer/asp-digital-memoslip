namespace AspDigitalMemoSlip.Application.Exceptions
{
    public class QrCodeGenerationException : Exception
    {
        public QrCodeGenerationException() : base()
        {

        }

        public QrCodeGenerationException(string message) : base(message)
        {

        }
    }
}
