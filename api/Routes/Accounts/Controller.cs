using System.Security.Claims;
using AutoMapper;
using Ecommerce.Share.Controller;
using Ecommerce.Share.Model;
using Ecommerce.Share.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Routes.Accounts;


public class AccountController : BaseAPIController
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ITokenService tokenService;
    private readonly IMapper mapper;

    public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, IMapper mapper)
        : base(logger)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
        if (email == null)
            return BadRequest(new ErrorResponse(400));

        var user = await userManager.FindByEmailAsync(email);

        return Ok(new UserDTO
        {
            Email = email,
            Token = tokenService.CreateToken(user!),
            DisplayName = user!.DisplayName
        });
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
    {
        return await userManager.FindByEmailAsync(email) != null;
    }

    [HttpGet("address")]
    [Authorize]
    public async Task<IActionResult> GetUserAddress()
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email)?.ToUpper();
        if (email == null)
            return Unauthorized(new ErrorResponse(401));

        var user = await userManager.Users.AsNoTracking().Include(u => u.Address).SingleOrDefaultAsync(u => u.NormalizedEmail == email);

        return Ok(mapper.Map<Address, AddressDTO>(user!.Address!));
    }

    [HttpPut("address")]
    [Authorize]
    public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email)?.ToUpper();
        if (email == null)
            return Unauthorized(new ErrorResponse(401));

        var user = await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.NormalizedEmail == email);
        user!.Address = mapper.Map<AddressDTO, Address>(address);
        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded) return mapper.Map<Address, AddressDTO>(user!.Address!);

        return BadRequest(new ErrorResponse("Can not update address"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return Unauthorized(new ErrorResponse("Email or Password are not recognized"));

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ErrorResponse("Email or Password are not recognized"));

        return Ok(new UserDTO
        {
            Email = user.Email,
            Token = tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        if ((await CheckEmailExists(registerDTO.Email)).Value)
        {
            return BadRequest(new ValidationErrorResponse
            {
                Errors = new string[] { "Email address in use" }
            });
        }

        var user = new ApplicationUser
        {
            DisplayName = registerDTO.DisplayName,
            Email = registerDTO.Email,
            UserName = registerDTO.Email
        };

        var result = await userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
            return BadRequest(new ErrorResponse(400));

        return Ok(new UserDTO
        {
            DisplayName = user.DisplayName,
            Token = tokenService.CreateToken(user),
            Email = user.Email
        });
    }

}