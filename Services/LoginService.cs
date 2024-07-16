using Incidencias.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Incidencias.Services
{
	public class LoginService
	{
		private readonly HttpClient httpClient;

		public LoginService(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<bool> Login(string username, string password)
		{
			var url = "https://www.incidenciascedimat.somee.com/api/login";
			var loginRequest = new LoginRequest
			{
				UserName = username,
				Password = password
			};

			var json = JsonSerializer.Serialize(loginRequest);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync(url, content);

			if(!response.IsSuccessStatusCode)
				return false;

			var jsonResult = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<LoginResponse>(jsonResult);

			Preferences.Set("token", result!.Token);
			Preferences.Set("username", result.UserName);
			Preferences.Set("name", result.Name);
			Preferences.Set("lastname", result.LasName);
			Preferences.Set("id", result.Id);

			return true;
		}
	}
}
