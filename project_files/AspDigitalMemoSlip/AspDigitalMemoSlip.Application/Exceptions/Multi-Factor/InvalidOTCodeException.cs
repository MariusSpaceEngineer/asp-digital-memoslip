namespace AspDigitalMemoSlip.Application.Exceptions.Multi_Factor
{
    public class InvalidOTCodeException : Exception
    {
        public InvalidOTCodeException(string message) : base(message) { }
    }
}
