using Ecommerce.Shared;

namespace Ecommerce.Module.ProductBrands;

[Route("api/products/brands")]
public class ProductBrandsController : APIController
{
    private readonly IRepository<ProductBrand, int> productBrandRepository;

    public ProductBrandsController(ILogger<ProductBrandsController> logger, IRepository<ProductBrand, int> productBrandRepository)
        :base(logger)
    {
        this.productBrandRepository = productBrandRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductBrands()
    {
        var productBrands = await productBrandRepository.GetAllAsync();
        return Ok(productBrands);
    }
}