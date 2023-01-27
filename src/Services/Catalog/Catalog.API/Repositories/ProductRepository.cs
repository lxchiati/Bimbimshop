using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task CreateNewAsync(Product product)
        {
           await   _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _catalogContext.Products.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await    _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var updateResult = await _catalogContext.Products.ReplaceOneAsync(filter: f => f.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleteResult = await _catalogContext.Products.DeleteOneAsync(id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount> 0;
        }


    }
}
