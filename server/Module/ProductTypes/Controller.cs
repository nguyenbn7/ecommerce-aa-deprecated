using Ecommerce.Shared;

namespace Ecommerce.Module.ProductTypes;

[Route("api/products/types")]
public class ProductTypesController : APIController
{
    private readonly IRepository<ProductType, int> productTypeRepository;

    public ProductTypesController(ILogger<ProductTypesController> logger, IRepository<ProductType, int> productTypeRepository)
        : base(logger)
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