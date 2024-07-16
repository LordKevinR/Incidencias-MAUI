using Incidencias.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Incidencias.Services
{
	public class IncidenciasServices: IIncidenciasServices
	{
		HttpClient client;
		JsonSerializerOptions serializerOptions;
		string baseUrl = "https://www.incidenciascedimat.somee.com/api";
		private readonly IIncidenciasServices incidenciasServices;

		public ObservableCollection<IncidenciaModel> Incidencias { get; set; } = new ObservableCollection<IncidenciaModel>();
		public ObservableCollection<IncidenciaModel> IncidenciasCompletas { get; set; } = new ObservableCollection<IncidenciaModel>();
		public ObservableCollection<IncidenciaModel> IncidenciasPrioritarias { get; set; } = new ObservableCollection<IncidenciaModel>();
		public ObservableCollection<IncidenciaModel> IncidenciasArchivadas { get; set; } = new ObservableCollection<IncidenciaModel>();


		public string Codigo { get; set; } = string.Empty;
		public int CodigoInt { get; set; }
		public string Descripcion { get; set; } = string.Empty;
		public string ReportadaPor { get; set; } = string.Empty;
		public bool EsPrioridad { get; set; }
		public string Comentario { get; set; } = string.Empty;

		public bool isRefreshing { get; set; }
		public async Task GetAllIncidencias()
		{
			var url = $"{baseUrl}/incidencias";
			var response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				using (var responseStream = await response.Content.ReadAsStreamAsync())
				{
					var data = await JsonSerializer.DeserializeAsync<ObservableCollection<IncidenciaModel>>(responseStream, serializerOptions);

					if (data != null)
					{
						Incidencias = new ObservableCollection<IncidenciaModel>(data.Where(incidencia => !incidencia.isCompleted && !incidencia.contractor && incidencia.canWorkNow).Reverse());

						IncidenciasCompletas = new ObservableCollection<IncidenciaModel>(data.Where(incidencia => incidencia.isCompleted).Reverse());

						IncidenciasPrioritarias = new ObservableCollection<IncidenciaModel>(data.Where(incidencia => incidencia.contractor).Reverse());

						IncidenciasArchivadas = new ObservableCollection<IncidenciaModel>(data.Where(incidencia => !incidencia.canWorkNow).Reverse());
					}
				}
			}
		}

		public async Task RefreshCommand()
		{
			isRefreshing = true;
			await GetAllIncidencias();
			isRefreshing = false;
		}


	}
}
