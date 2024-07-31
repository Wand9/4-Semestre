using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo1.Domains;
using minimalAPIMongo1.Services;
using minimalAPIMongo1.ViewModel;
using MongoDB.Driver;

namespace minimalAPIMongo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                //Lista todos os pedidos da collection "Order"
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

                //percorre todos os itens da lista
                foreach (var order in orders)
                {
                    //verificar se existe uma lista de produtos para cada pedido
                    if (order.ProductId != null)
                    {
                        //dentro da collection "Product" faz um filtro ("separa" os produtos que estão dentro do pedido
                        //olha, selecione os ids dos produtos dentro da collection cujo id esta presente na lista "order.ProductId"
                        var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductId);

                        //busca os produtos correspondentes ao pedido e adiciona em "order.Products"
                        //traz as informações dos produtos
                        order.Products = await _product.Find(filter).ToListAsync();
                    }
                    //busca e associa o cliente correspondente ao pedido
                    if (order.ClientId != null)
                    {
                       order.Client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();
                    }
                }
                return Ok(orders);              
            }
            catch (Exception e)
            {
                
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(string id)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();

                return order == null ? NotFound(new { Message = "Product not found" }) : Ok(order);
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
                var order = await _order.DeleteOneAsync(p => p.Id == id);

                return order == null ? NotFound(new { Message = "Product not found" }) : Ok(order);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderViewModel orderViewModel)
        {
            try
            {
                //instancia do pedido
                Order order = new Order();

                //atribuindo valores do objeto view model para o objeto cadastrado
                order.Id = orderViewModel.Id;
                order.Date = orderViewModel.Date;
                order.Status = orderViewModel.Status;
                order.ProductId = orderViewModel.ProductId;
                order.ClientId = orderViewModel.ClientId;

                //
                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound("Cliente nao encontrado !");
                }

                order.Client = client;

                //cadastre o pedido na collection
                await _order.InsertOneAsync(order);

                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Order updatedOrder)
        {
            try
            {
                var order = await _order.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (order == null)
                {
                    return NotFound();
                }

                updatedOrder.Id = order.Id;
                await _order.ReplaceOneAsync(p => p.Id == id, updatedOrder);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
