namespace Ecommerce.Module.Orders.Model;

public class ProductItemOrdered
{
    public ProductItemOrdered()
    {
    }

    public ProductItemOrdered(int productItemId,
                              string productName,
                              string pictureUrl)
    {
        ProductItemId = productItemId;
        ProductName = productName;
        PictureUrl = pictureUrl;
    }

    public int ProductItemId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
}
