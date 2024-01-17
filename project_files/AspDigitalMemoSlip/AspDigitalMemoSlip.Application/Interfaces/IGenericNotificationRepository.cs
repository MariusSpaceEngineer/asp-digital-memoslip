using AspDigitalMemoSlip.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IGenericNotificationRepository
    {
        Task<ICollection<GenericNotification>> GetAllNotificationUser(string id);
        Task<GenericNotification> Create(GenericNotification item);
        
    }
}
