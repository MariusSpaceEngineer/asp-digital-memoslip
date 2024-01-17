using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MemoSlipContext context;
        
        public NotificationRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Notification>> GetAllNotificationForConsigner(string id)
        {
            var notifications = await context.Notifications
                .Where(n => n.ConsignerId == id)
                .ToListAsync();

            return notifications;
        }

        public async Task<Notification> Create(Notification item)
        {
            await context.Notifications.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
