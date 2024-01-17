using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class ConsigneeRepository : IConsigneeRepository
    {
        private readonly MemoSlipContext context;

        public ConsigneeRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<Consignee> GetConsigneeByMemoId(int id)
        {
            var memo = await context.Memos
                                    .Include(m => m.Consignee)
                                    .FirstOrDefaultAsync(m => m.Id == id);

            return memo?.Consignee;
        }

        public async Task<IEnumerable<Consignee>> GetAll(int pageNr, int pageSize)
        {
            return await context.Consignees.Skip((pageNr - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Consignee>> GetAll()
        {
            return await context.Consignees.ToListAsync();
        }

        public async Task<Consignee> GetById(string id)
        {
            return await context.Consignees.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Consignee> Create(Consignee newConsignee)
        {
            await context.Consignees.AddAsync(newConsignee);
            return newConsignee;
        }

        public Consignee Update(Consignee modified)
        {
            context.Consignees.Update(modified);
            return modified;
        }


        public void Delete(Consignee consignee)
        {
            context.Consignees.RemoveRange(consignee);
        }

    
        public async Task<Consignee> GetConsigneeByUserId(string userId)
        {
            return await context.Consignees.FirstOrDefaultAsync(c => c.Id == userId);
            
        }

        public async Task<bool> UpdateConsigneeAcceptedByConsignerToTrue(string userId)
        {
            var consignee = await context.Consignees.FirstOrDefaultAsync(c => c.Id == userId);
            if (consignee == null)
            {
                return false;
            }

            consignee.AcceptedByConsigner = true;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteConsignee(string userId)
        {
            var consignee = await context.Consignees.FindAsync(userId);
            if (consignee == null)
            {
                return false;
            }

            context.Consignees.Remove(consignee);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
