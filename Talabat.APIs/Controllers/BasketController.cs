using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Reposotories.Centext;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository ,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet] // ../api/basket?id=
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsnc(id);
            
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost] // ../api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
        var mappedBasket = _mapper.Map<CustomerBasketDto , CustomerBasket>(basket);
           var createdOrUpdated =  await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdated is null) return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdated);
        }
        [HttpDelete] // ../api/basket
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

        }
    }
}
