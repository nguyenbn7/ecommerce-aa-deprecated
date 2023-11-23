using Ecommerce.Share;

namespace Ecommerce.API.ProductType.Controller;

[Route("api/products/types")]
public class ProductTypesController : BaseAPIController
{
    private readonly IRepository<Model.ProductType, int> productTypeRepository;

    public ProductTypesController(IRepository<Model.ProductType, int> productTypeRepository)
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