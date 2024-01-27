using Azure.Core;
using Azure;
using CQRS.Domain.Entities;
using CQRS.Domain.IRepositories;
using CQRS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CQRS.Application.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CQRSContext context;

        public ProductRepository(CQRSContext context)
        {
            this.context = context;
        }
        public async Task AddProductAsync(Product product)
        {
            product.Id = Guid.NewGuid();
            product.CreateDate = DateTime.UtcNow;
            product.Deleted = false;

            await context.Products.AddAsync(product);
        }

        public async Task<Guid> GetCreatorUserId(Guid productId)
        {
            var product = await GetProductByIdAsync(productId);
            return product.CreatorPerson;
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await GetProductByIdAsync(productId);
            product.Deleted = true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await context.Products.SingleOrDefaultAsync(p=> p.Id == productId);
        }

        public async void UpdateProduct(Product product,Guid productId)
        {
            Product currentProduct = await GetProductByIdAsync(productId);

            currentProduct.ManufacturePhone = product.ManufacturePhone;
            currentProduct.ManufactureEmail = product.ManufactureEmail;
            currentProduct.ProduceDate = product.ProduceDate;
            currentProduct.LastUpdateDate = DateTime.UtcNow;
            currentProduct.IsAvailable = product.IsAvailable;
            currentProduct.Description = product.Description;
            currentProduct.Title = product.Title;
        }
    }
}
