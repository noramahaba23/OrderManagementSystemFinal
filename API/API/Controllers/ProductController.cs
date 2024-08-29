using Microsoft.AspNetCore.Mvc;
using API.Hub;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;




namespace API.API
{
    public class ProductController : ControllerBase
    {
        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string Description { get; set; }
        }

        private readonly Send sender;
        private readonly IHubContext<Class> _hubContext;
        private readonly IDatabase _redisDb;

        public ProductController(IHubContext<Class> hubContext, IConnectionMultiplexer redis)
        {

            sender = new Send("localhost", "hello");
            _hubContext = hubContext;
            _redisDb = redis.GetDatabase();

        }


        // POST api/values
        [HttpPost("/send")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            const string message = "new post product request";
            sender.SendMessage(message); // RMQ
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);


            if (string.IsNullOrEmpty(product.Id))
            {
                return BadRequest("Product ID is required.");
            }

            // Save product to Redis
            await _redisDb.StringSetAsync(product.Id, JsonSerializer.Serialize(product));

            return CreatedAtAction(nameof(GetAllProducts), new { id = product.Id }, product);
        }

        //GET API
        [HttpGet("/all")]
        public async Task<IActionResult> GetAllProducts()
        {
            const string message = "new get all reqest";
            sender.SendMessage(message);

            var server = _redisDb.Multiplexer.GetServer(_redisDb.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys();

            var products = new List<Product>();
            foreach (var key in keys)
            {
                var productJson = await _redisDb.StringGetAsync(key);
                if (!productJson.IsNullOrEmpty)
                {
                    var product = JsonSerializer.Deserialize<Product>(productJson);
                    products.Add(product);
                }
            }

            return Ok(products);
        }

    }
}

