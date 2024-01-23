using Ecommerce.Shared.Database;
using Ecommerce.Shared.Model;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Testing;

[ApiController]
[Route("api/[controller]")]
public class BuggyController : ControllerBase
{
    private AppDbContext Context { get; }

    public BuggyController(AppDbContext context)
    {
        Context = context;
    }

    [HttpGet("testauth")]
    [Authorize]
    public IActionResult GetTextSecret()
    {
        return Ok("KaBOOOMMM!");
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFoundRequest()
    {
        var thing = Context.Products.Find(42);
        if (thing == null)
        {
            return NotFound(new ErrorResponse(StatusCodes.Status404NotFound));
        }
        return Ok();
    }

    [HttpGet("server-error")]
    public IActionResult GetServerErrorRequest()
    {
        var thing = Context.Products.Find(42);
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