using AutoMapper;
using Ecommerce.Shared;

namespace Ecommerce.Module.Products;

public class ProductsController : APIController
{
    private readonly IRepository<Product, int> productRepository;
    private readonly IMapper mapper;
    private readonly List<IIncludeSpecification<Product>> includes = new()
    {
        new IncludeProductBrand(),
        new IncludeProductType()
    };

    public ProductsController(ILogger<ProductsController> logger, IRepository<Product, int> productRepository, IMapper mapper)
        : base(logger)
    {
        this.productRepository = productRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Page<ProductDTO>>> GetProducts([FromQuery] ProductsParam @params)
    {
        var spec = new ProductsPredicate(@params.BrandId, @params.TypeId, @params.Search);

        var sorts = new List<Sort<Product>>();
        if (!string.IsNullOrEmpty(@params.Sort))
        {
            switch (@params.Sort.ToLower())
            {
                case "price":
                    sorts.Add(new(p => p.Price));
                    break;
                case "-price":
                    sorts.Add(new(p => p.Price, SortDirection.DESC));
                    break;
                default:
                    sorts.Add(new(p => p.Name));
                    break;
            }
        }
        else
        {
            sorts.Add(new(p => p.Name));
        }

        var pageProduct = await productRepository.GetAllAsync(Pageable.Of(@params.PageIndex, @params.PageSize), includes, spec, sorts);

        return new Page<ProductDTO>
        {
            PageIndex = pageProduct.PageIndex + 1,
            PageSize = pageProduct.PageSize,
            TotalItems = pageProduct.TotalItems,
            Data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(pageProduct.Data)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var spec = new ProductPredicate(id);
        var product = await productRepository.GetOneAsync(includes, spec);
        if (product == null)
            return NotFound(new ErrorResponse("Product does not exist"));
        return mapper.Map<Product, ProductDTO>(product);
    }
}