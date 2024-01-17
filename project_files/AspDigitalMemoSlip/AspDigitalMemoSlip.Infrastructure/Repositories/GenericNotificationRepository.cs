using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    internal class GenericNotificationRepository : IGenericNotificationRepository
    {
        private readonly MemoSlipContext context;

        public GenericNotificationRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<GenericNotification>> GetAllNotificationUser(string id)
        {
            var notifications = await context.GenericNotifications
                .Where(n => n.ReceiverId == id)
                .ToListAsync();

            return notifications;
        }

        public async Task<GenericNotification> Create(GenericNotification item)
        {
            await context.GenericNotifications.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }
    }
}
