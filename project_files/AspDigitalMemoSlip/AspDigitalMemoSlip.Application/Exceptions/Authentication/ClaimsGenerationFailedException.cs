namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class ClaimsGenerationFailedException : Exception
    {
        public ClaimsGenerationFailedException()
        {
        }

        public ClaimsGenerationFailedException(string message)
            : base(message)
        {
        }

        public ClaimsGenerationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
