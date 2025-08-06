using GymSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using Microsoft.AspNetCore.Connections;

namespace GymSystem.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CallBackController : ControllerBase
	{


		private readonly GymSystemContext _context;

		private readonly ILogger<CallBackController> _logger;

		private readonly IConfiguration _configuration;

		private readonly UserManager<ApiUser> _userManager;

		private readonly SignInManager<ApiUser> _signInManager;

		public CallBackController(
			GymSystemContext context,
			ILogger<CallBackController> logger,
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

		[HttpGet]
		public async Task<IActionResult> GetAuthorizationCode(string code)
		{
			//var token= "v^1.1#i^1#p^3#r^0#I^3#f^0#t^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";

			//ApiContext context=new ApiContext();
			//context.ApiCredential.eBayToken = token;

			//context.SoapApiServerUrl = "https://api.ebay.com/wsapi";
				
			



			_logger.LogInformation("the call back methode started");
			_logger.LogInformation("and the code is "+ code);
			return Ok(new {message="suceed with code "+code });

		}

	}
}
