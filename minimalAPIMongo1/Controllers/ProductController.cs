using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo1.Domains;
using minimalAPIMongo1.Services;
using MongoDB.Driver;

namespace minimalAPIMongo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<Product> _product;

        /// <summary>
        /// Construtor que recebe como dependencia o objeto da classe MongoDbService
        /// </summary>
        /// <param name="mongoDbService">Objeto da classe MongoDbService</param>
        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var product = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                await _product.InsertOneAsync(product);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                var product = await _product.Find(filter).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound($"Produto com ID '{id}' não encontrado.");
                }
                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Product updatedProduct)
        {
            try
            {
                var product = await _product.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (product == null)
                {
                    return NotFound();
                }

                updatedProduct.Id = product.Id;
                await _product.ReplaceOneAsync(p => p.Id == id, updatedProduct);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _product.DeleteOneAsync(p => p.Id == id);
                if (result.DeletedCount == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
