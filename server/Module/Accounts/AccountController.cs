using System.Security.Claims;
using AutoMapper;
using Ecommerce.Module.Accounts.DTO;
using Ecommerce.Module.Accounts.Model;
using Ecommerce.Module.Accounts.Model.Response;
using Ecommerce.Shared;
using Ecommerce.Shared.Model;
using Ecommerce.Shared.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Module.Accounts;

public class AccountController : APIController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AuthenticationService _authenticationTokenService;
    private readonly IMapper _mapper;

    public AccountController(
        ILogger<AccountController> logger,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        AuthenticationService authenticationTokenService,
        IMapper mapper)
        : base(logger)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticationTokenService = authenticationTokenService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
        if (email == null)
            return BadRequest(new ErrorResponse(400));

        var user = await _userManager.FindByEmailAsync(email);

        return Ok(new CustomerInfoWithToken
        {
            Email = email,
            Token = _authenticationTokenService.CreateAccessToken(user!),
            DisplayName = user!.DisplayName
        });
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [HttpGet("address")]
    [Authorize]
    public async Task<IActionResult> GetUserAddress()
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email)?.ToUpper();
        if (email == null)
            return Unauthorized(new ErrorResponse(401));

        var user = await _userManager.Users.AsNoTracking()
            .Include(u => u.Address).SingleOrDefaultAsync(u => u.NormalizedEmail == email);

        return Ok(_mapper.Map<Address, CustomerAddress>(user!.Address!));
    }

    [HttpPut("address")]
    [Authorize]
    public async Task<ActionResult<CustomerAddress>> UpdateUserAddress(CustomerAddress address)
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email)?.ToUpper();
        if (email == null)
            return Unauthorized(new ErrorResponse(401));

        var user = await _userManager.Users.Include(u => u.Address)
            .SingleOrDefaultAsync(u => u.NormalizedEmail == email);

        user!.Address = _mapper.Map<CustomerAddress, Address>(address);

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded) return _mapper.Map<Address, CustomerAddress>(user!.Address!);

        return BadRequest(new ErrorResponse("Can not update address"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CustomerLogin customerLogin)
    {
        var unauthorizedResponse = new ErrorResponse("Email or Password are not recognized");

        var user = await _userManager.FindByEmailAsync(customerLogin.Email);
        if (user == null)
            return Unauthorized(unauthorizedResponse);

        var result = await _signInManager.CheckPasswordSignInAsync(user, customerLogin.Password, false);

        if (!result.Succeeded)
            return Unauthorized(unauthorizedResponse);

        return Ok(new CustomerInfoWithToken
        {
            Email = user.Email,
            Token = _authenticationTokenService.CreateAccessToken(user),
            DisplayName = user.DisplayName
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CustomerRegister customerRegister)
    {
        if ((await CheckEmailExists(customerRegister.Email)).Value)
        {
            return BadRequest(new ValidationErrorResponse
            {
                Errors = new string[] { "Email address in use" }
            });
        }

        var user = new AppUser
        {
            DisplayName = customerRegister.DisplayName,
            Email = customerRegister.Email,
            UserName = customerRegister.Email
        };

        var result = await _userManager.CreateAsync(user, customerRegister.Password);

        if (!result.Succeeded)
            return BadRequest(new ErrorResponse(400));

        return Ok(new CustomerInfoWithToken
        {
            DisplayName = user.DisplayName,
            Token = _authenticationTokenService.CreateAccessToken(user),
            Email = user.Email
        });
    }

}