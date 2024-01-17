namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class UserCreationFailedException : Exception
    {
        public UserCreationFailedException()
        {
            
        }

        public UserCreationFailedException(string message) : base(message)
        {

        }
    }
}
