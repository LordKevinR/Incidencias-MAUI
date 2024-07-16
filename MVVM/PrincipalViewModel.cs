using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Incidencias.MVVM.Models;

namespace Incidencias.MVVM
{
    public class PrincipalViewModel
	{
		HttpClient client;
		JsonSerializerOptions serializerOptions;
		string baseUrl = "http://www.incidenciascedimat.somee.com/api";

		public PrincipalViewModel()
        {
            client = new HttpClient();
			serializerOptions = new JsonSerializerOptions 
			{ 
				WriteIndented = true,
			};
        }

        public ICommand GetAllIncidenciasCommand => new Command( async () =>
		{
			var url = $"{baseUrl}/incidencias";
			var response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				//var content = await response.Content.ReadAsStringAsync();
				using(var responseStream = await response.Content.ReadAsStreamAsync())
				{
					var data = await JsonSerializer.DeserializeAsync<List<IncidenciaModel>>(responseStream, serializerOptions);
				}
			}

		});

		public ICommand GetSingleIncidenciaCommand => new Command( async () =>
		{
			var url = $"{baseUrl}/incidencias/1234";
			var response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				//var content = await response.Content.ReadAsStringAsync();
				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					var data = await JsonSerializer.DeserializeAsync<IncidenciaModel>(responseStream, serializerOptions);
				}
			}

		});

		public ICommand PostIncidenciaCommand => new Command(async () =>
		{
			
			var url = $"{baseUrl}/incidencias";

			var incidencia = new IncidenciaModel
			{
				created = DateTime.Now,
				createdByUser = "Melvin",
				code = 112233,
				descripcion = "Plafon primer piso",
				isCompleted = false,
				canWorkNow = true,
				contractor = false,
				closedBy = "Kevin"
			};

			string json = JsonSerializer.Serialize<IncidenciaModel>(incidencia, serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync(url, content);

		});
	}
}
