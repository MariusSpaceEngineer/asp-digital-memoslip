namespace AspDigitalMemoSlip.Application.Exceptions.Consignee
{
    public class ConsigneeDeletionFailedException : Exception
    {
        public ConsigneeDeletionFailedException(string message) : base(message)
        {
        }
        public ConsigneeDeletionFailedException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
