namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class UserTypeNotRecognizedException : Exception
    {
        public UserTypeNotRecognizedException()
        {
        }

        public UserTypeNotRecognizedException(string message)
            : base(message)
        {
        }
    }
}
