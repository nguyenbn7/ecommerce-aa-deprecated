using AutoMapper;
using Ecommerce.Share.Controller;

namespace Ecommerce.API.Baskets;

public class BasketController : BaseAPIController
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper mapper;

    public BasketController(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketById(string id)
    {
        var basket = await _basketRepository.GetBasketAsync(id);
        return Ok(basket ?? new CustomerBasket(id));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasket(CustomerBasketDTO basketDTO)
    {
        var basket = mapper.Map<CustomerBasketDTO, CustomerBasket>(basketDTO);
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