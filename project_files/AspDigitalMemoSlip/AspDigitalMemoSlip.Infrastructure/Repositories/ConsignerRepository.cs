using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class ConsignerRepository : IConsignerRepository
    {
        private readonly MemoSlipContext context;

        public ConsignerRepository(MemoSlipContext context)
        {
            this.context = context;
        }
        public async Task<Consigner> GetByUserName(string name)
        {
            return await context.Consigners.FirstOrDefaultAsync(c => c.NormalizedUserName == name);
        }
        public Task<Consigner> Create(Consigner Item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Consigner Item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Consigner>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Consigner>> GetAll(int PageNr, int PageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Consigner> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Consigner> GetById(string Id)
        {
            var consigner = await context.Consigners.FindAsync(Id);
            return consigner;
        }

        public Consigner Update(Consigner Modified)
        {
            throw new NotImplementedException();
        }


        void IGenericRepository<Consigner, string>.Delete(Consigner Item)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Consigner>> IGenericRepository<Consigner, string>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Consigner>> IGenericRepository<Consigner, string>.GetAll(int PageNr, int PageSize)
        {
            throw new NotImplementedException();
        }


        public async Task<Consigner> GetConsingerByMemoId(int memoId)
        {
           var memo = await context.Memos
                                    .Include(m => m.Consigner)
                                    .FirstOrDefaultAsync(m => m.Id == memoId);
            return memo?.Consigner;
        }


        public async Task<List<Consignee>> GetPendingConsigneesByConsignerId(string consignerId)
        {
            return await context.Consignees
            .Where(c => c.ConsignerId == consignerId && !c.AcceptedByConsigner)
            .ToListAsync();

        }

        public async Task<Consigner> GetConsginerByConsigneeId(string consigneeId)
        {
            return await context.Consigners
                .FirstOrDefaultAsync(c => c.Consignees.Any(consignee => consignee.Id.Equals(consigneeId)));
        }

        public async Task<Consigner> FindById(string id)
        {
            return await context.Consigners.FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}
