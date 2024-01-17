using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AspDigitalMemoSlip.Infrastructure.Contexts;
using DTOClassLibrary.DTO.Product;
using Microsoft.EntityFrameworkCore;
using System;

namespace AspDigitalMemoSlip.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MemoSlipContext context;

        public ProductRepository(MemoSlipContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByMemoId(int memoId)
        {
            var memoWithProducts = await context.Memos
                                                .Include(m => m.Products)
                                                .SingleOrDefaultAsync(m => m.Id == memoId);
            return memoWithProducts?.Products ?? new List<Product>();
        }


        public Task<Product> Create(Product Item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product Item)
        {
            context.Remove(Item);
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
        }






        Product IGenericRepository<Product, int>.Update(Product Modified)
        {
            context.Products.Update(Modified);
            context.SaveChangesAsync();

            return Modified;
        }

        public async Task<List<Product>> UpdateMultipleProducts(List<Product> modifiedProducts)
        {
            context.Products.UpdateRange(modifiedProducts);
            await context.SaveChangesAsync();

            return modifiedProducts;
        }



        Task<Product> IGenericRepository<Product, int>.Create(Product Item)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Product>> GetAll(int pageNr, int pageSize)
        {
            return await context.Products.Skip((pageNr - 1) * pageSize).Take(pageSize).ToListAsync();
        }



        public async Task<Product> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<Product> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> UpdateProductsForSalesConfirmation(IEnumerable<Product> productsInSalesConfirmation)
        {
            var transaction = context.Database.BeginTransaction();

            try
            {
                foreach (var modifiedEntity in productsInSalesConfirmation)
                {
                    context.Products.Update(modifiedEntity);
                }

                await context.SaveChangesAsync();

                transaction.Commit();
                return productsInSalesConfirmation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                throw;
            }
        }
        public async Task<Product> UpdateCaratOfAProduct(ProductSale incProduct)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == incProduct.ProductId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            product.Carat -= incProduct.CaratsSold;
           
            context.Products.Update(product);

            await context.SaveChangesAsync();
            return product;
        }
        public async Task<IEnumerable<Product>> GetProductsBySalesConfirmation(int salesConfirmationId)
        {
            var products = await context.Products.Where(p => p.SalesConfirmationId == salesConfirmationId).ToListAsync();
            return products;
        }

        
    }
}
