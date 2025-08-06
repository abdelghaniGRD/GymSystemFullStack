
using GymSystem.DTO;
using GymSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.DTO
{
	public class MemberDTO
	{
		public int Id { get; set; }

		
		public string Name { get; set; }=string.Empty;
		
		public DateOnly Birthday { get; set; }

		public int phone { get; set; }

		public string idNumber { get; set; }=string.Empty;

		public string Addresse { get; set; } = string.Empty;

		public DateTime JoinDate { get; set; }

		public string AspNetUser { get; set; } = string.Empty;

		public virtual ICollection<Attendance> Attendances { get; set; }
			= new List<Attendance>();

		public ICollection<Subscription>susbcriptions { get; set; }=
			new List<Subscription>();

		//public string filterQuery {  get; set; } = string.Empty;
		//public int pageIndex { get; set; } = 0;

		//public int pageSize { get; set; } = 10;

		//public int recordCount { get; set; }






	}
}
