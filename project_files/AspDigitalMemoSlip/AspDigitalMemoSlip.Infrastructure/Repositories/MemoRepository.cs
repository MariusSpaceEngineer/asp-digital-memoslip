using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class MemoRepository : IMemoRepository
    {
        private readonly MemoSlipContext context;

        public MemoRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<Memo> Create(Memo item)
        {
            await context.Memos.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }



        public void Delete(Memo item)
        {
            context.Memos.Remove(item);
            context.SaveChanges();
        }

        public async Task<IEnumerable<Memo>> GetAll()
        {
            var memos = await context.Memos.ToListAsync();
            return memos;
        }



        public async Task<IEnumerable<Memo>> GetAll(int pageNr, int pageSize)
        {
            return await context.Memos
                               .Skip((pageNr - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();
        }

        public async Task<Memo> GetById(int id)
        {
            return await context.Memos.FindAsync(id);
        }

        public Task<Memo> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public Memo Update(Memo modified)
        {
            var memo = context.Memos.Update(modified);
            context.SaveChanges();
            return memo.Entity;
        }

        public async Task<List<Memo>> GetMemosByConsigneeId(string consigneeId)
        {
            return await context.Memos
                                 .Where(m => m.ConsigneeId == consigneeId)
                                 .Include(m => m.Consigner) 
                                 .Include(m => m.Consignee) 
                                 .Include(m => m.Products)  
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Memo>> GetMemosWithProductsByConsginee(string consgineeId)
        {
            return await context.Memos.Where(m => m.ConsigneeId == consgineeId)
                .Where(m => m.AcceptedByConsignee)
                .Include(m => m.Consigner)
                .Include(m => m.Products)
                .ToListAsync();
        }

    }

}
