using AutoMapper;
using Ecommerce.Module.Products.DTO;
using Ecommerce.Module.Products.Model;
using Ecommerce.Module.Products.Specification;
using Ecommerce.Shared;
using Ecommerce.Shared.Database;
using Ecommerce.Shared.Model;

namespace Ecommerce.Module.Products;

public class ProductsController : APIController
{
    private readonly Repository<Product, int> _productRepository;
    private readonly Repository<ProductType, int> _productTypeRepository;
    private readonly Repository<ProductBrand, int> _productBrandRepository;
    private readonly IMapper _mapper;

    private readonly List<IIncludeSpecification<Product>> includes = new()
    {
        new IncludeProductBrand(),
        new IncludeProductType()
    };

    public ProductsController(
        ILogger<ProductsController> logger,
        Repository<Product, int> productRepository,
        Repository<ProductType, int> productTypeRepository,
        Repository<ProductBrand, int> productBrandRepository,
        IMapper mapper)
        : base(logger)
    {
        _productRepository = productRepository;
        _productTypeRepository = productTypeRepository;
        _productBrandRepository = productBrandRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Page<ProductReponse>>> GetProducts([FromQuery] ProductsParam @params)
    {
        var spec = new ProductSpecification();

        if (@params.BrandId.HasValue)
        {
            spec.And(new GetProductBrandByBrandId(@params.BrandId.Value));
        }

        if (@params.TypeId.HasValue)
        {
            spec.And(new GetProductTypeByTypeId(@params.TypeId.Value));
        }

        if (@params.Search != null)
        {
            spec.And(new GetProductBySearchTerm(@params.Search));
        }

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

        var pageProduct = await _productRepository.GetAllAsync(Pageable.Of(@params.PageIndex, @params.PageSize), spec, includes, sorts);

        return new Page<ProductReponse>
        {
            PageIndex = pageProduct.PageIndex + 1,
            PageSize = pageProduct.PageSize,
            TotalItems = pageProduct.TotalItems,
            Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductReponse>>(pageProduct.Data)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReponse>> GetProduct(int id)
    {
        var spec = new ProductPredicate(id);
        var product = await _productRepository.GetOneAsync(includes, spec);
        if (product == null)
            return NotFound(new ErrorResponse("Product does not exist"));
        return _mapper.Map<Product, ProductReponse>(product);
    }

    [HttpGet("Types")]
    public async Task<IActionResult> GetProductTypes()
    {
        var productTypes = await _productTypeRepository.GetAllAsync();
        return Ok(productTypes);
    }

    [HttpGet("Brands")]
    public async Task<IActionResult> GetProductBrands()
    {
        var productBrands = await _productBrandRepository.GetAllAsync();
        return Ok(productBrands);
    }
}