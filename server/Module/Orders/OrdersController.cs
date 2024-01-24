using System.Security.Claims;
using AutoMapper;
using Ecommerce.Module.Orders.Model;
using Ecommerce.Shared;
using Ecommerce.Shared.Model.Response;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Module.Orders;

[Authorize]
public class OrdersController : APIController
{
    private readonly OrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(ILogger<OrdersController> logger,
                            OrderService orderService,
                            IMapper mapper) : base(logger)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync(OrderDTO orderDTO)
    {
        var buyerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

        if (buyerEmail == null)
        {
            return BadRequest(new ApiError("Problem creating order"));
        }

        var address = _mapper.Map<AddressDTO, Address>(orderDTO.ShipToAddress);

        var order = await _orderService.CreateOrderAsync(buyerEmail,
                                                         orderDTO.DeliveryMethodId,
                                                         orderDTO.BasketId,
                                                         address);

        if (order == null)
        {
            return BadRequest(new ApiError("Problem creating order"));
        }
        return Ok(order);
    }
}