using Ecommerce.Share.Controller;
using Ecommerce.Share.GenericRepository;

namespace Ecommerce.Routes.ProductBrands;

[Route("api/products/brands")]
public class ProductBrandsController : BaseAPIController
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