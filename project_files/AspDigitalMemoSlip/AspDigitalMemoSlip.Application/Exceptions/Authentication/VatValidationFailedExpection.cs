namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class VatValidationFailedExpection : Exception
    {
        public VatValidationFailedExpection()
        {
            
        }

        public VatValidationFailedExpection(string message, Exception inner)
         : base(message, inner)
        {
        }
    }
}
