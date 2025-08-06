﻿namespace GymSystem.DTO
{
	public class RestDTO<T>
	{
		
		public T Data { get; set; } = default!;
		public int? PageIndex { get; set; }
		public int? PageSize { get; set; }
		public int? RecordCount { get; set; }
	}
}
