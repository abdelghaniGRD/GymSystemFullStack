using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace GymSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{



		[HttpGet]
		public async Task<IActionResult>getallcategories()
		{
			string token = ""; // Set your OAuth token here
			

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string url = $"https://api.ebay.com/commerce/taxonomy/v1/category_tree/0";
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
