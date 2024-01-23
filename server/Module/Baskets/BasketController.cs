using AutoMapper;
using Ecommerce.Module.Baskets.DTO;
using Ecommerce.Module.Baskets.Model;
using Ecommerce.Shared;

namespace Ecommerce.Module.Baskets;

public class BasketController : APIController
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketController(ILogger<BasketController> logger,
                            IBasketRepository basketRepository,
                            IMapper mapper)
        : base(logger)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketById(string id)
    {
        var basket = await _basketRepository.GetBasketAsync(id);
        return Ok(basket ?? new Basket(id));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasket(CustomerBasket customerBasket)
    {
        var basket = _mapper.Map<CustomerBasket, Basket>(customerBasket);
        var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
        return Ok(updatedBasket);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBasketById(string id)
    {
        var deleted = await _basketRepository.DeleteBasketAsync(id);
        if (deleted)
            return Ok();
        return NotFound();
    }
}