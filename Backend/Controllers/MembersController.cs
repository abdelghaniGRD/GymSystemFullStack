using GymSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymSystem.DTO;
using System.ComponentModel;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace GymSystem.Controllers
{
	[Route("[controller]")]
	[ApiController]
	
	public class MembersController : ControllerBase
	{

		private ILogger<Member> _logger;
		private GymSystemContext _context;
		private UserManager<ApiUser> _userManager;

		public MembersController(
			GymSystemContext gymSystemContext
			, ILogger<Member> logger,
			UserManager<ApiUser> userManager)
		{
			_context = gymSystemContext;
			_logger = logger;
			_userManager = userManager;
		}

		[Authorize]
		[HttpGet]
		public async Task<RestDTO<Member[]>> Get([FromQuery] int PageIndex,
												 string? filterQuery = null)
		{
			
			

			var result = _context.Members.AsQueryable();

			if (!string.IsNullOrEmpty(filterQuery))
			{
				result = result.Where(m => m.Name.Contains(filterQuery));
			}

			var AspUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (AspUserId != null)
			{
				result =
				result.Where(m => m.AspNetUserId == AspUserId)
											  .OrderBy(m => m.Name)
											  .Skip(PageIndex * 10)
											  .Take(20);
				//result = result.


			return new RestDTO<Member[]>
			{
				Data = await result.ToArrayAsync(),
				PageIndex = PageIndex,
				PageSize = 10,
				RecordCount = await _context.Members.Where(
						m=>m.AspNetUserId == AspUserId).CountAsync()

			};
			}
			else
			{
				throw new Exception("not found");
			}
		}

		[HttpGet("{id}")]
		public async Task<RestDTO<Member>> GetMember(int id)
		{
			var results = await _context.Members.Where(m => m.AspNetUserId ==
						  User.FindFirst(ClaimTypes.NameIdentifier).Value)
						  .FirstOrDefaultAsync(m => m.Id == id);

			return new RestDTO<Member>()
			{
				Data = results
			};
		}

		[HttpPost]
		public async Task<ActionResult> AddMember(MemberDTO model)
		{
			var aspuserid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (aspuserid == null)
			{
				return Unauthorized("aspuserid not found");
			}

			if (await _context.Members.Where(m => m.Name == model.Name &&
				model.AspNetUser==aspuserid)
										 .FirstOrDefaultAsync() != null)
			{
				return BadRequest("member already registred");
			}
			else
			{
				Member MB = new Member();

				if (ModelState.IsValid)
				{

					MB.Name = model.Name;
					MB.AspNetUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

					MB.Birthday = model.Birthday;

					MB.Phone = model.phone;
					MB.IdNumber = model.idNumber;
					MB.Addresse = model.Addresse;

					MB.JoinDate = DateOnly.FromDateTime(DateTime.Now);

					if (!string.IsNullOrEmpty(model.AspNetUser))
					{
						MB.AspNetUserId = model.AspNetUser;
					}

					_context.Members.Add(MB);
					await _context.SaveChangesAsync();
				}

				return Ok(new { MB.Id });


			}
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var member = await _context.Members.
				Where(m => m.AspNetUserId == User.FindFirst(ClaimTypes.NameIdentifier).Value
										&& m.Id == id)
				.FirstOrDefaultAsync();

			if (member != null)
			{
				_context.Members.Remove(member);
				await _context.SaveChangesAsync();

				return Ok("user deleted");
			}
			return NotFound();
		}

		[HttpPut]
		public async Task<ActionResult> Edit(MemberDTO model)
		{
			_logger.LogInformation("edit user " + User.FindFirst(
				ClaimTypes.NameIdentifier).Value);
			
			_logger.LogInformation("member is"+model.Id);


			try
			{
				var member = await _context.Members.Where(m => m.AspNetUserId
							== User.FindFirst(ClaimTypes.NameIdentifier).Value
											&& m.Id == model.Id)
					.FirstOrDefaultAsync();


				if (member == null)
				{
					return NotFound();
				}

				member.Name = model.Name;
				member.Phone = model.phone;
				member.Birthday = model.Birthday;
				member.Addresse = model.Addresse;
				member.IdNumber = model.idNumber;

				_context.Members.Update(member);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}

		}

	}
}
