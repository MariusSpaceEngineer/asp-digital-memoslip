namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class InvalidVatNumberException : Exception
    {
        public InvalidVatNumberException()
        {
        }

        public InvalidVatNumberException(string message)
            : base(message)
        {
        }
    }
}
