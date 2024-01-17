namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class UserValidationFailedException : Exception
    {
        public UserValidationFailedException()
        {
        }

        public UserValidationFailedException(string message)
            : base(message)
        {
        }
    }
}
