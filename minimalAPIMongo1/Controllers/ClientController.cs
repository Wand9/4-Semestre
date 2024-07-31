using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo1.Domains;
using minimalAPIMongo1.Services;
using MongoDB.Driver;

namespace minimalAPIMongo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<User> _user;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var client = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(client);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> Get(string id)
        {
            try
            {
                var client = await _client.Find(x => x.Id == id).FirstOrDefaultAsync();

                return client == null ? NotFound(new { Message = "Product not found" }) : Ok(client);
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
                var client = await _client.DeleteOneAsync(p => p.Id == id);

                return client == null ? NotFound(new { Message = "Product not found" }) : Ok(client);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Client client)
        {
            try
            {
                _client.InsertOneAsync(client);
                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Client updatedClient)
        {
            try
            {
                var client = await _client.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound();
                }

                updatedClient.Id = client.Id;
                await _client.ReplaceOneAsync(p => p.Id == id, updatedClient);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}