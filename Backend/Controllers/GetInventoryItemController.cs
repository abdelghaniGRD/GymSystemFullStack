using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GymSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GetInventoryItemController : Controller
	{
		[HttpGet]
		public async Task<IActionResult >GetInventoryItem(string SKU)
		{
			string token = "v^1.1#i^1#p^3#I^3#r^0#f^0#t^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"; // Set your OAuth token here


			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string url = $"https://api.ebay.com/sell/inventory/v1/inventory_item/{SKU}";
				var response = await client.GetAsync(url);


				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();

					var formattedJson = JsonConvert.SerializeObject(
										JsonConvert.DeserializeObject(data),
													Formatting.Indented);
					Console.Write(formattedJson);

					return Ok(formattedJson);
				}
				else
				{
					Console.WriteLine("Error: " + response.ReasonPhrase);
					return BadRequest();
				}
			}
		}
	}
}
