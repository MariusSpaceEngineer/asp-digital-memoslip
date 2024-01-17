namespace AspDigitalMemoSlip.Application.Exceptions.Email
{
    public class EmailSendingException : Exception
    {
        public EmailSendingException()
        {
        }

        public EmailSendingException(string message)
            : base(message)
        {
        }

        public EmailSendingException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
