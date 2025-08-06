using GymSystem.DTO;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymSystem.Controllers
{

	[Route("[controller]/[action]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		private readonly GymSystemContext _context;

		private readonly ILogger<AccountController> _logger;

		private readonly IConfiguration _configuration;

		private readonly UserManager<ApiUser> _userManager;

		private readonly SignInManager<ApiUser> _signInManager;

		public AccountController(
			GymSystemContext context,
			ILogger<AccountController> logger,
			IConfiguration configuration,
			UserManager<ApiUser> userManager,
			SignInManager<ApiUser> signInManager)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
			_context = context;
			_configuration = configuration;
		}


		[HttpPost]
		[ResponseCache(NoStore = true)]

		public async Task<ActionResult> Register(RegisterDTO input)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newUser = new ApiUser();

					newUser.UserName = input.UserName;
					newUser.Email = input.Email;
					newUser.PhoneNumber = input.PhoneNumber;
					

					var result = await _userManager.CreateAsync(
						newUser, input.Password);

					if (result.Succeeded)
					{
						_logger.LogInformation(
						"User {userName} ({email}) has been created.",
						newUser.UserName, newUser.Email);

						return StatusCode(201,
						$"User '{newUser.UserName}' has been created.");
					}
					else
						throw new Exception(
							string.Format("Error: {0}", string.Join(" ",
								result.Errors.Select(e => e.Description))));
				}
				else
				{
					var details = new ValidationProblemDetails(ModelState);
					details.Type =
							"https://tools.ietf.org/html/rfc7231#section-6.5.1";
					details.Status = StatusCodes.Status400BadRequest;
					return new BadRequestObjectResult(details);
				}
			}
			catch (Exception e)
			{
				var exceptionDetails = new ProblemDetails();
				exceptionDetails.Detail = e.Message;
				exceptionDetails.Status =
					StatusCodes.Status500InternalServerError;
				exceptionDetails.Type =
						"https://tools.ietf.org/html/rfc7231#section-6.6.1";
				return StatusCode(
					StatusCodes.Status500InternalServerError,
					exceptionDetails);
			}
		}


		[HttpPost]
		[ResponseCache(NoStore = true)]
		public async Task<ActionResult> Login([FromBody] LoginDTO input)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var user = await _userManager.FindByNameAsync(input.UserName);

					if (user == null
						|| !await _userManager.CheckPasswordAsync(
												user, input.Password))

						return Unauthorized("Invalid login attempt.");

					else
					{
						var signingCredentials =

						new SigningCredentials(new SymmetricSecurityKey(
										System.Text.Encoding.UTF8.GetBytes(
										_configuration["JWT:SigningKey"])),

										SecurityAlgorithms.HmacSha256);

						var claims = new List<Claim>();
						claims.Add(new Claim(ClaimTypes.Name, user.UserName));
						claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

						var jwtObject = new JwtSecurityToken(
						issuer: _configuration["JWT:Issuer"],
						audience: _configuration["JWT:Audience"],
						claims: claims,
						expires: DateTime.UtcNow.AddHours(4),
						signingCredentials: signingCredentials);


						var jwtString = new JwtSecurityTokenHandler()
						.WriteToken(jwtObject);

						Response.Cookies.Append("AuthToken", jwtString, new CookieOptions
						{
							HttpOnly = true, // Ensures the cookie is inaccessible to JavaScript
							Secure = true,   // Requires HTTPS
							SameSite = SameSiteMode.None, // Prevents CSRF attacks
							Expires = DateTime.UtcNow.AddHours(4) // Expiration time for the cookie
							
						});

						return Ok(new {message="logged success",jwt=jwtString});
					}
				}
				else
				{
					var details = new ValidationProblemDetails(ModelState);
					details.Type =
					"https://tools.ietf.org/html/rfc7231#section-6.5.1";
					details.Status = StatusCodes.Status400BadRequest;
					return new BadRequestObjectResult(details);
				}

			}
			catch (Exception e)
			{
				var exceptionDetails = new ProblemDetails();
				exceptionDetails.Detail = e.Message;
				exceptionDetails.Status =
				StatusCodes.Status401Unauthorized;
				exceptionDetails.Type =
				"https://tools.ietf.org/html/rfc7231#section-6.6.1";
				return StatusCode(
				StatusCodes.Status401Unauthorized,
				exceptionDetails);
			}
		}

		[HttpPost]
		public async Task <IActionResult>Logout()
		{
			Response.Cookies.Delete("AuthToken", new CookieOptions
			{
				Path = "/",
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.None,
			});

			await _signInManager.SignOutAsync();

			return Ok(new {message="Logged Out successfully"});
		}
	}
}
