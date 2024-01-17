using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using DTOClassLibrary.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class SalesConfirmationRepository : ISalesConfirmationRepository
    {
        private readonly MemoSlipContext context;

        public SalesConfirmationRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<SalesConfirmationDTO> Create(string ConsignerId,string ConsigneeId, SalesConfirmationDTO salesConfirmation)
        {
            var newSalesConfirmation = new SalesConfirmation
            {
                CreatedDate = DateTime.Now,
                ConsigneeId = ConsigneeId,
                ConsignerId = ConsignerId,
                SuggestedCommision = salesConfirmation.SuggestedCommision,
                PaymentTermDays = salesConfirmation.PaymentTermDays
            };
            context.SalesConfirmations.Add(newSalesConfirmation);
            await context.SaveChangesAsync();
            salesConfirmation.Id = newSalesConfirmation.Id;

            return salesConfirmation;
        }

        public async Task<IEnumerable<SalesConfirmation>> GetAllSalesConfirmationsForUser(string userId)
        {
            var salesConfirmations = await context.SalesConfirmations
                .Where(s => s.ConsigneeId == userId || s.ConsignerId == userId)
                .Include(s => s.ProductSales)
                .ToListAsync();

            return salesConfirmations;
        }

        public async Task<SalesConfirmation> GetSalesConfirmationById(int id)
        {
            var result = await this.context.SalesConfirmations.FindAsync(id);
            return result;
        }

        public async Task<SalesConfirmation> Update(SalesConfirmation salesConfirmation)
        {
            context.SalesConfirmations.Update(salesConfirmation);
            await context.SaveChangesAsync();
            return salesConfirmation;
        }
    }
}
