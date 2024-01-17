using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOClassLibrary.DTO.Product
{
    public class ProductStateChangeDTO
    {
        public List<ProductStateUpdate> ProductStateUpdates { get; set; }
    }

    public class ProductStateUpdate
    {
        public int ProductId { get; set; }
        public int MemoId { get; set; }
        public ProductState ProductState { get; set; }
        public string? Remark { get; set; }
    }

}
