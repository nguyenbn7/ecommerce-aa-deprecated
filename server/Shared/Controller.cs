namespace Ecommerce.Shared;

[ApiController]
[Route("api/[controller]")]
public abstract class APIController : ControllerBase
{
    protected readonly ILogger _logger;

    protected APIController(ILogger logger)
    {
        _logger = logger;
    }
}
