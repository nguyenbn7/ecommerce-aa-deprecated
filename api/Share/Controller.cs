namespace Ecommerce.Share.Controller;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseAPIController : ControllerBase
{
    private readonly ILogger? logger;

    protected BaseAPIController()
    {

    }
    protected BaseAPIController(ILogger logger)
    {
        this.logger = logger;
    }
}