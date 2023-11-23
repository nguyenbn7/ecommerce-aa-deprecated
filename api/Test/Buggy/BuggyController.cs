using Ecommerce.Core.Database;
using Ecommerce.Share.Model;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce_ASP_NET_Core.Buggy;

[ApiController]
[Route("api/[controller]")]
public class BuggyController : ControllerBase
{
    private readonly StoreContext storeContext;

    public BuggyController(StoreContext storeContext)
    {
        this.storeContext = storeContext;
    }

    [HttpGet("testauth")]
    [Authorize]
    public IActionResult GetTextSecret() {
        return Ok("KaBOOOMMM!");
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFoundRequest()
    {
        var thing = storeContext.Products.Find(42);
        if (thing == null)
        {
            return NotFound(new ErrorResponse(StatusCodes.Status404NotFound));
        }
        return Ok();
    }

    [HttpGet("server-error")]
    public IActionResult GetServerErrorRequest()
    {
        var thing = storeContext.Products.Find(42);
#nullable disable
        var thingToReturn = thing.ToString();
#nullable enable
        return Ok(thingToReturn);
    }

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest(new ErrorResponse(StatusCodes.Status400BadRequest));
    }

    [HttpGet("bad-request/{id}")]
    public IActionResult GetBadRequest(int id)
    {
        return BadRequest(new ErrorResponse(id.ToString()));
    }
}