using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly MemoSlipContext context;

        public InvoiceRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Invoice>> GetAll(int pageNr, int pageSize)
        {

            return await context.Invoices.Include(p => p.SaleConfirmation).Skip((pageNr - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            return await context.Invoices.ToListAsync();
        }

        public async Task<Invoice> GetById(int id)
        {
            return await context.Invoices.Where(p => p.Id == id).Include(p => p.SaleConfirmation).FirstOrDefaultAsync();
        }

        public async Task<Invoice> Create(Invoice newInvoice)
        {
            // Validate if newInvoice is not null before proceeding
            if (newInvoice == null)
            {
                throw new ArgumentNullException(nameof(newInvoice), "The provided invoice object is null.");
            }

            await context.Invoices.AddAsync(newInvoice);
            // Save changes to the database
            await context.SaveChangesAsync();  // This line is crucial

            return newInvoice;
        }

        public Invoice Update(Invoice modified)
        {
            context.Invoices.Update(modified);
            // Save changes to the database
            context.SaveChanges();

            return modified;
        }

        public void Delete(Invoice invoice)
        {
            context.Invoices.RemoveRange(invoice);
            // Save changes to the database
            context.SaveChanges();
        }

        public Task<Invoice> GetById(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
