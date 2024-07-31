using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo1.Domains;
using minimalAPIMongo1.Services;
using MongoDB.Driver;
using System.Collections.ObjectModel;

namespace minimalAPIMongo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var user = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            try
            {
                var user = await _user.Find(x => x.Id == id).FirstOrDefaultAsync();

                return user == null ? NotFound(new { Message = "Product not found" }) : Ok(user);
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
                var user = await _user.DeleteOneAsync(p => p.Id == id);

                return user == null ? NotFound(new { Message = "Product not found" }) : Ok(user);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            try
            {
                _user.InsertOneAsync(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            try
            {
                var user = await _user.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                updatedUser.Id = user.Id;
                await _user.ReplaceOneAsync(p => p.Id == id, updatedUser);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}
