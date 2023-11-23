using AutoMapper;
using Ecommerce.Share;
using Ecommerce.Share.Model;
using Ecommerce.Share.Specification;

namespace Ecommerce.API.Products;

public class ProductsController : BaseAPIController
{
    private readonly IProductRepository productRepository;
    private readonly IMapper mapper;

    public ProductsController(IProductRepository productRepository, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Page<ProductDTO>>> GetProducts([FromQuery] ProductsParam @params)
    {
        var spec = new ProductsSpecification(@params.BrandId, @params.TypeId, @params.Search);
        var sorts = new List<Sort<Product, int>>();

        var pageProduct = await productRepository.GetAllAsync(Pageable.Of(@params.PageIndex, @params.PageSize), spec);

        return new Page<ProductDTO>
        {
            PageIndex = pageProduct.PageIndex,
            PageSize = pageProduct.PageSize,
            TotalItems = pageProduct.TotalItems,
            Data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(pageProduct.Data)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var spec = new ProductSpecification(id);
        var product = await productRepository.GetOneAsync(spec);
        if (product == null)
            return NotFound(new ErrorResponse("Product does not exist"));
        return mapper.Map<Product, ProductDTO>(product);
    }
}