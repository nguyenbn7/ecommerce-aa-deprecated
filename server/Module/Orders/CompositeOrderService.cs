using Ecommerce.Module.Baskets;
using Ecommerce.Module.Orders.Model;
using Ecommerce.Module.Products.Model;
using Ecommerce.Shared.Database;

namespace Ecommerce.Module.Orders;

public class CompositeOrderService : OrderService
{
    private readonly Repository<Order, int> _orderRepo;
    private readonly Repository<DeliveryMethod, int> _deliveryMethodRepo;
    private readonly Repository<Product, int> _productRepo;
    private readonly IBasketRepository _basketRepo;

    public CompositeOrderService(Repository<Order, int> orderRepo,
                                 Repository<DeliveryMethod, int> deliveryMethodRepo,
                                 Repository<Product, int> productRepo,
                                 IBasketRepository basketRepo)
    {
        _orderRepo = orderRepo;
        _deliveryMethodRepo = deliveryMethodRepo;
        _productRepo = productRepo;
        _basketRepo = basketRepo;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail,
                                        int deliveryMethodId,
                                        string basketId,
                                        Address shippingAddress)
    {
        var basket = await _basketRepo.GetBasketAsync(basketId);

        if (basket == null)
        {
            throw new Exception("Basket not found");
        }

        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _productRepo.GetByIdAsync(item.Id);
            if (productItem == null)
            {
                throw new Exception($"Can not find product with id: {item.Id}");
            }

            var itemOrdered = new ProductItemOrdered
            {
                ProductItemId = productItem.Id,
                ProductName = productItem.Name,
                PictureUrl = productItem.PictureUrl
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrdered,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            items.Add(orderItem);
        }

        var deliveryMethod = await _deliveryMethodRepo.GetByIdAsync(deliveryMethodId);

        if (deliveryMethod == null)
        {
            throw new Exception("Delivery method not found");
        }

        var subTotal = items.Sum(item => item.Price * item.Quantity);

        var order = new Order
        {
            OrderItems = items,
            BuyerEmail = buyerEmail,
            DeliveryMethod = deliveryMethod,
            ShipToAddress = shippingAddress,
            SubTotal = subTotal
        };

        // TODO: add to db

        return order;
    }

    public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new NotImplementedException();
    }
}