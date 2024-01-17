namespace AspDigitalMemoSlip.Application.Exceptions.Consigner
{
    public class ConsignerNotFoundException : Exception
    {
        public ConsignerNotFoundException()
        {

        }

        public ConsignerNotFoundException(string message) : base(message)
        {
        }
    }
}
