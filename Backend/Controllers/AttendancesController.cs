using GymSystem.DTO;
using GymSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{

	[Route("[controller]")]
	[ApiController]

	public class AttendancesController : ControllerBase
	{
		private GymSystemContext _context;
		public AttendancesController(GymSystemContext context)
		{
			_context = context;
		}


		[HttpPost]
		public async Task<ActionResult> Add(Attendance attendance)
		{
			if (ModelState.IsValid)
			{
				Subscription? subscription = new Subscription();

				subscription = await _context.Subscriptions
							  .Where(s => s.MemberId == attendance.MemberId)
							  .OrderBy(o => o.Id)
							  .LastOrDefaultAsync();
				if (subscription?.EndDate <= DateOnly.FromDateTime(DateTime.Now))
				{
					return BadRequest("the subscription is expired");
				}

				await _context.Attendances.AddAsync(attendance);

				await _context.SaveChangesAsync();

				return Ok(new { attendance.Id });

			}
			return BadRequest();

		}

		[HttpGet("{id}")]
		public async Task<RestDTO<Attendance[]>> Get(int id)
		{
			var query = _context.Attendances.Where(a => a.MemberId == id);

			return new RestDTO<Attendance[]>
			{
				Data = await query.ToArrayAsync()
			};

		}


	}
}
