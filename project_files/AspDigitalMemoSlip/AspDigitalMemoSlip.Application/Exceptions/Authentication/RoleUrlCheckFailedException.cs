namespace AspDigitalMemoSlip.Application.Exceptions.Authentication
{
    public class RoleUrlCheckFailedException : Exception
    {
        public RoleUrlCheckFailedException()
        {
        }

        public RoleUrlCheckFailedException(string message)
            : base(message)
        {
        }
    }
}
