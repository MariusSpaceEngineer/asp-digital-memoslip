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
    public class ProductSaleRepository : IProductSaleRepository
    {
        private readonly MemoSlipContext context;

        public ProductSaleRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<ProductSale> Create(ProductSale item)
        {
            this.context.ProductsSale.Add(item); 
            await this.context.SaveChangesAsync(); 

            return item;
        }

        public void Delete(ProductSale Item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductSale>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductSale>> GetAll(int PageNr, int PageSize)
        {
            throw new NotImplementedException();   
        }

        public async Task<ProductSale> GetById(int Id)
        {
            var result = await this.context.ProductsSale.FirstOrDefaultAsync(x => x.Id == Id);
            return result;
        }

        ProductSale IGenericRepository<ProductSale, int>.Update(ProductSale Modified)
        {
            this.context.ProductsSale.Update(Modified);
            return Modified;
        }
    }
}
