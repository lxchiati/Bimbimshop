using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/Catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly ILogger<CatalogController> logger;
        
        public CatalogController(IProductRepository repo, ILogger<CatalogController> logger)
        {
            this.repository = repo;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var products = await repository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> GetProductById(string id) {
            var product = await repository.GetByIdAsync(id);
            if(product == null)
            {
                logger.LogError($"Product with id {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet]
        [Route("[action]/{category}", Name ="GetProductByCategory")]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCateogy(string category) {
            logger.LogInformation($"Find Product with cate {category}");
            var products = await repository.GetByCategoryAsync(category);
            
            return Ok(products);
        }

        [HttpGet]
        [Route("[action]/{name}", Name = "GetProductByName")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            logger.LogInformation($"Find Product with Name {name}");
            var products = await repository.GetByNameAsync(name);

            return Ok(products);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await repository.CreateNewAsync(product);
            return CreatedAtRoute("GetProduct", new {id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await repository.UpdateAsync(product));
        }
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProduct(string productId)
        {
            return Ok(await repository.DeleteAsync(productId));
        }

    }
}