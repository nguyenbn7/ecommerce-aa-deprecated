namespace Ecommerce.Module.Baskets.Model;

public class Basket
{
    public Basket()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Basket(string id)
    {
        Id = id;
    }
    public string Id { get; set; }

    
    public List<BasketItem> Items { get; set; } = new();
}