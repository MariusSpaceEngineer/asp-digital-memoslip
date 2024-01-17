using Microsoft.AspNetCore.Mvc;

namespace AspDigitalMemoSlip.Application.Utils
{
    public class UserImagesResult
    {
        public FileContentResult IDCopy { get; set; }
        public FileContentResult Selfie { get; set; }

    }
}
