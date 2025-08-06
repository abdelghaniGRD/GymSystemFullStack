using Microsoft.AspNetCore.Identity;

namespace GymSystem.Models
{
	public class ApiUser:IdentityUser
	{

		public ICollection<Member>? Members { get; set; }
	}
}
