using AutoMapper;
using Ecommerce.Module.Products.DTO;
using Ecommerce.Module.Products.Model;
using Ecommerce.Module.Products.Specification;
using Ecommerce.Shared;
using Ecommerce.Shared.Database;
using Ecommerce.Shared.Database.Specification;
using Ecommerce.Shared.Model;
using Ecommerce.Shared.Model.Pagination;
using Ecommerce.Shared.Model.Response;

namespace Ecommerce.Module.Products;

public class ProductsController : APIController
{
    private readonly Repository<Product, int> _productRepository;
    private readonly Repository<ProductType, int> _productTypeRepository;
    private readonly Repository<ProductBrand, int> _productBrandRepository;
    private readonly IMapper _mapper;

    public ProductsController(ILogger<ProductsController> logger,
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
        var includedProperties = new List<Includable<Product>>()
        {
            new(p => p.ProductBrand), new(p => p.ProductType)
        };

        var predicates = new ProductSpecification() as Specificational<Product>;
        if (@params.BrandId.HasValue)
            predicates = predicates.And(new GetProductBrandByBrandId(@params.BrandId.Value));

        if (@params.TypeId.HasValue)
            predicates = predicates.And(new GetProductTypeByTypeId(@params.TypeId.Value));

        if (@params.Search != null)
            predicates = predicates.And(new GetProductBySearchTerm(@params.Search));

        var orderedProperties = new List<Orderable<Product>>();
        if (!string.IsNullOrEmpty(@params.Sort))
            switch (@params.Sort.ToLower())
            {
                case "price":
                    orderedProperties.Add(new(p => p.Price));
                    break;
                case "-price":
                    orderedProperties.Add(new(p => p.Price, OrderDirection.DESC));
                    break;
                default:
                    orderedProperties.Add(new(p => p.Name));
                    break;
            }
        else
            orderedProperties.Add(new(p => p.Name));


        var pageProduct = await _productRepository.GetAllAsync(includedProperties,
                                                               predicates,
                                                               orderedProperties,
                                                               Pageable.Of(@params.PageNumber, @params.PageSize));

        return new Page<ProductReponse>
        {
            PageNumber = pageProduct.PageNumber,
            PageSize = pageProduct.PageSize,
            TotalItems = pageProduct.TotalItems,
            Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductReponse>>(pageProduct.Data)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReponse>> GetProduct(int id)
    {
        var includedProperties = new List<Includable<Product>>()
        {
            new(p => p.ProductBrand), new(p => p.ProductType)
        };

        var predicates = new ProductSpecification(id);

        var product = await _productRepository.GetFirstOrDefaultAsync(includedProperties,
                                                                      predicates);

        if (product == null)
            return NotFound(new ApiError("Product does not exist"));

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