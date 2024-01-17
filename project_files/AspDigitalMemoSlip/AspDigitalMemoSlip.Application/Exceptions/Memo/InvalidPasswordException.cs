namespace AspDigitalMemoSlip.Application.Exceptions.Memo
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string message) : base(message) { }
    }
}
