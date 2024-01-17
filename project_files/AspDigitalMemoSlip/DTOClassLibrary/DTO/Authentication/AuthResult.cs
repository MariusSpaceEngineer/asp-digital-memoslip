namespace DTOClassLibrary.DTO.Authentication
{
    public class AuthResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        public AuthResult(int statusCode, string message, string token = null)
        {
            StatusCode = statusCode;
            Message = message;
            Token = token;
        }
    }


}
