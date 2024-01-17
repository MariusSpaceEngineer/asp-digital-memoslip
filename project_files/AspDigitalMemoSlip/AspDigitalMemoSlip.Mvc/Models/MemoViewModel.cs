using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspDigitalMemoSlip.Mvc.Models
{
    public class MemoViewModel
    {
        public Memo Memo { get; set; }

        [BindNever]
        public List<SelectListItem>? Consignees { get; set; }
    }
}
