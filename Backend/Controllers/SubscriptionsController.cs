using GymSystem.DTO;
using GymSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymSystem.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{

		private GymSystemContext _context;
		private ILogger<Subscription> _logger;

		public SubscriptionsController(GymSystemContext context,ILogger<Subscription> logger)
		{
			_context = context;
			_logger = logger;
		}


		//[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
		[Authorize]
		[HttpGet("{id}")]
		public async Task<RestDTO<Subscription[]>> Get(int id)
		{

			_logger.LogInformation("get subscription action methode executed ");
			var aspUser=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			try
			{

				var query = await _context.Subscriptions.Where(s=>s.MemberId==id)
														.ToArrayAsync();

				
				return new RestDTO<Subscription[]>
				{
					Data =query,

				};
			}
			catch (Exception ex) 
			{
				Console.WriteLine( "this is the message error " + ex.Message);
				return new RestDTO<Subscription[]>
					{
						Data = [],

					};

			}
		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> Add(SubscriptionDTO model)
		{
			if (ModelState.IsValid)
			{
				Console.WriteLine( "we recieve member id" +
					model.memberId + "and the pla id: " + model.planID);
				try
				{

				Plan? plan = new Plan();

				var aspUser=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				plan = await _context.Plans.Where(p => p.Id == model.planID
												  &&p.AspNetUser==aspUser)
											.FirstOrDefaultAsync();

				Subscription subscription = new Subscription();

				subscription.StartDate = DateOnly.FromDateTime(DateTime.Now);

				//subscription.Status = model.status;
				subscription.MemberId = model.memberId;
				subscription.EndDate = DateOnly.FromDateTime( DateTime.Now.AddMonths(plan.DurationInMonths));
				subscription.PlanId = model.planID;
				subscription.PlanName=plan.Name;
				subscription.PlanPrice = plan.Price;

				await _context.Subscriptions.AddAsync(subscription);
				await _context.SaveChangesAsync();

					
				}
				catch (Exception ex) 
				{
					return BadRequest(new {message=ex.Message});

				}
			}
			return Ok(new { message = "subscription added" });


		}

	}
}
