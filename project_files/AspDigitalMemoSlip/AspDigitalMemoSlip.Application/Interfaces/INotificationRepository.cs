using AspDigitalMemoSlip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<ICollection<Notification>> GetAllNotificationForConsigner(string id);
        Task<Notification> Create(Notification item);
    }
}
