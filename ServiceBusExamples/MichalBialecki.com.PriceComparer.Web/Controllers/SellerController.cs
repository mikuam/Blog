using System;
using System.Threading.Tasks;
using MichalBialecki.com.PriceComparer.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MichalBialecki.com.PriceComparer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerRepository _sellerRepository;

        public SellerController(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(string id)
        {
            try
            {
                var seller = await _sellerRepository.Get(id);
                return new JsonResult(seller);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public async Task Post([FromBody] Seller seller)
        {
            try
            {
                await _sellerRepository.Save(seller);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("{id}/mark/{mark}")]
        public async Task AddMark(string id, decimal mark)
        {
            try
            {
                var seller = await _sellerRepository.Get(id);
                if (seller == null)
                {
                    return;
                }

                seller.MarksCount += 1;
                seller.MarksSum += mark;

                await _sellerRepository.Update(seller);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
