using Ecommerce.Share;
using Ecommerce.Share.GenericRepository;

namespace Ecommerce.API.ProductTypes;

[Route("api/products/types")]
public class ProductTypesController : BaseAPIController
{
    private readonly IRepository<ProductType, int> productTypeRepository;

    public ProductTypesController(IRepository<ProductType, int> productTypeRepository)
    {
        this.productTypeRepository = productTypeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductTypes()
    {
        var productTypes = await productTypeRepository.GetAllAsync();
        return Ok(productTypes);
    }
}