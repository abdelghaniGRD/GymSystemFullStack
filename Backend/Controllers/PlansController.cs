using GymSystem.DTO;
using GymSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GymSystem.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PlansController : ControllerBase
	{
		GymSystemContext _context;
		public PlansController(GymSystemContext gymSystemContext)
		{
			_context = gymSystemContext;
		}

		[Authorize]
		[HttpGet]
		public async Task<RestDTO<Plan[]>> Get()
		{

			
			var aspUser=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var query = _context.Plans.Where(p=>p.AspNetUser==aspUser).AsQueryable();

			return new RestDTO<Plan[]>
			{
				Data = await query.ToArrayAsync(),
				RecordCount =await query.Where(p=>p.AspNetUser==aspUser).CountAsync()
			};
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<RestDTO<Plan>> GetPlan(int id)
		{

			var query = _context.Plans.AsQueryable();

			Plan result;

			result = await _context.Plans.FirstOrDefaultAsync(mb => mb.Id == id);

			return new RestDTO<Plan>()
			{
				Data = result
			};
		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> AddPlan(PlanDTO model)
		{
			var aspUser=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (await _context.Plans.Where(m => m.Name == model.name &&
											m.Name== aspUser)
										 .FirstOrDefaultAsync() != null)
			{
				return BadRequest(new {message="plan already exist"});
			}
			
				try
				{

					Plan plan = new Plan();

					if (ModelState.IsValid)
						{

						plan.Name = model.name;

						plan.DurationInMonths = model.durationInMonths;

						plan.Price = model.price;

					//if (!string.IsNullOrEmpty(model.aspNetUser))
					//{
					//	plan.AspNetUser = model.aspNetUser;
					//}
						plan.AspNetUser =aspUser;

						_context.Plans.Add(plan);

						await _context.SaveChangesAsync();

					}
						return Ok(new {planId= plan.Id,message="plan Added" });
				}
				catch (Exception ex) 
				{ 
					return BadRequest(new {message=ex.Message});
				}

			
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var aspUser=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var plan = await _context.Plans.Where(p=>p.AspNetUser==aspUser)
										   .FirstOrDefaultAsync(p => p.Id == id);

			if (plan != null)
			{
				_context.Plans.Remove(plan);
				await _context.SaveChangesAsync();

				return Ok(new {message= "plan deleted "});
			}

			return NotFound();
		}



	}
}
