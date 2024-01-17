namespace AspDigitalMemoSlip.Application.Exceptions.Memo
{
    public class MemoNotFoundException : Exception
    {
        public MemoNotFoundException(string message) : base(message) { }
    }
}
