using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO.Product;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<IEnumerable<Product>> GetProductsByMemoId(int id);
        Task<IEnumerable<Product>> UpdateProductsForSalesConfirmation(IEnumerable<Product> productsInSalesConfirmation);
        Task<IEnumerable<Product>> GetProductsBySalesConfirmation(int salesConfirmationId);
        Task<Product> GetProductById(int id);
        Task<Product> UpdateCaratOfAProduct(ProductSale product);
        Task<List<Product>> UpdateMultipleProducts(List<Product> modifiedProducts);
    }
}
