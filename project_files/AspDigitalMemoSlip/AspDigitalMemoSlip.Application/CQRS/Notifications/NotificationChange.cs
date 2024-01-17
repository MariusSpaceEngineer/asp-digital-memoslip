using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Domain
{
    public class NotificationChange
    {
        public int Id { get; set; }
        public string ConsginerId { get; set; }
        public string ConsgineeId { get; set; }
        public bool IsRead { get; set; }
    }
}
