using System.ComponentModel.DataAnnotations;

namespace GymSystem.DTO
{
	public class PlanDTO
	{
		[Required]
		public string name { get; set; }

		public int durationInMonths {  get; set; }

		public int price {  get; set; }

		public string? aspNetUser {  get; set; }


	}
}
