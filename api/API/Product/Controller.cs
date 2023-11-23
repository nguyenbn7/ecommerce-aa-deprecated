using AutoMapper;
using Ecommerce.API.Product.Controller.Param;
using Ecommerce.API.Product.Model;
using Ecommerce.API.Product.Specification;
using Ecommerce.Share;

namespace Ecommerce.API.Product.Controller;

public class ProductsController : BaseAPIController
{
    private readonly IRepository<Model.Product, int> productRepository;
    private readonly IMapper mapper;

    public ProductsController(IRepository<Model.Product, int> productRepository, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductsParam @params)
    {
        var spec = new ProductsSpecification(@params.BrandId, @params.TypeId, @params.Search);
        var sorts = new List<Sort<Model.Product, int>>();

        var pageProduct = await productRepository.GetAllAsync(Pageable.Of(@params.PageIndex, @params.PageSize), spec);

        return new Pagination<ProductDTO>
        {
            PageIndex = pageProduct.PageIndex,
            PageSize = pageProduct.PageSize,
            TotalItems = pageProduct.TotalItems,
            Data = mapper.Map<IReadOnlyList<Model.Product>, IReadOnlyList<ProductDTO>>(pageProduct.Data)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var spec = new ProductSpecification(id);
        var product = await productRepository.GetOneAsync(spec);
        if (product == null)
            return NotFound(new ErrorResponse("Product does not exist"));
        return mapper.Map<Model.Product, ProductDTO>(product);
    }
}