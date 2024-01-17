namespace AspDigitalMemoSlip.Application.Exceptions
{
    public class UpdateFailedException : Exception
    {
        public UpdateFailedException()
        {
        }

        public UpdateFailedException(string message)
            : base(message)
        {
        }
    }
}
