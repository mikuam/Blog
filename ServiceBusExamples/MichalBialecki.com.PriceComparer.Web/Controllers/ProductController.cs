using System;
using System.Threading.Tasks;
using MichalBialecki.com.PriceComparer.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MichalBialecki.com.PriceComparer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(string id)
        {
            try
            {
                var product = await _productRepository.Get(id);
                return new JsonResult(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task Post([FromBody] Product product)
        {
            try
            {
                await _productRepository.Save(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
