namespace AspDigitalMemoSlip.Application.Exceptions.Multi_Factor
{
    public class TotpGenerationException : Exception
    {
        public TotpGenerationException() : base()
        {

        }

        public TotpGenerationException(string message) : base(message)
        {

        }

        public TotpGenerationException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
