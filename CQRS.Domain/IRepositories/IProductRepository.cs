using CQRS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Domain.IRepositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(Guid productId);   
        
        Task<IEnumerable<Product>> GetAllProductsAsync();
        
        Task AddProductAsync(Product product);
        
        Task DeleteProductAsync(Guid productId);

        void UpdateProduct(Product product, Guid productId);

        Task<Guid> GetCreatorUserId(Guid productId);

    }
}
