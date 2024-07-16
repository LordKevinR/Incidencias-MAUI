using Incidencias.MVVM.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Incidencias.MVVM.ViewModels
{
	[AddINotifyPropertyChangedInterface]
	public class MainViewModel
	{
		HttpClient client;
		JsonSerializerOptions serializerOptions;
		string baseUrl = "https://www.incidenciascedimat.somee.com/api";
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
		public ICommand RefreshCommant => new Command(async () =>
		{
			isRefreshing = true;
			await GetAllIncidencias();
			isRefreshing = false;
		});



		public MainViewModel()
		{
			client = new HttpClient();
			serializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true,
			};


			GetAllIncidencias();
		}


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

						IncidenciasPrioritarias = new ObservableCollection<IncidenciaModel>(data.Where(incidencia => incidencia.contractor && incidencia.canWorkNow && !incidencia.isCompleted).Reverse());

						IncidenciasArchivadas = new ObservableCollection<IncidenciaModel>(data.Where(incidencia => !incidencia.canWorkNow && !incidencia.isCompleted).Reverse());
					}
				}
			}
		}

		public ICommand ArchivarCommand => new Command(async (inc) =>
		{
			IncidenciaModel incidencia = (IncidenciaModel)inc;

			var url = $"{baseUrl}/incidencias/{incidencia.id}";

			incidencia.canWorkNow = false;

			string json = JsonSerializer.Serialize<IncidenciaModel>(incidencia, serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync(url, content);

			await GetAllIncidencias();
		});
		
		public ICommand CompletarCommand => new Command(async (inc) =>
		{
			IncidenciaModel incidencia = (IncidenciaModel)inc;

			var url = $"{baseUrl}/incidencias/{incidencia.id}";

			incidencia.isCompleted = true;
			incidencia.canWorkNow = false;

			string json = JsonSerializer.Serialize<IncidenciaModel>(incidencia, serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync(url, content);

			await GetAllIncidencias();
		});			
		public ICommand EliminarCommand => new Command(async (inc) =>
		{
			IncidenciaModel incidencia = (IncidenciaModel)inc;

			var url = $"{baseUrl}/incidencias/{incidencia.id}";

			var response = await client.DeleteAsync(url);

			await GetAllIncidencias();
		});		
		public ICommand DesCompletarCommand => new Command(async (inc) =>
		{
			IncidenciaModel incidencia = (IncidenciaModel)inc;

			var url = $"{baseUrl}/incidencias/{incidencia.id}";

			incidencia.isCompleted = false;
			incidencia.canWorkNow = true;

			string json = JsonSerializer.Serialize<IncidenciaModel>(incidencia, serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync(url, content);

			await GetAllIncidencias();
		});

		public ICommand DesarchivarCommand => new Command(async (inc) =>
		{
			IncidenciaModel incidencia = (IncidenciaModel)inc;

			var url = $"{baseUrl}/incidencias/{incidencia.id}";

			incidencia.canWorkNow = true;
			incidencia.isCompleted = false;

			string json = JsonSerializer.Serialize<IncidenciaModel>(incidencia, serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync(url, content);

			await GetAllIncidencias();
		});

		public ICommand PostIncidenciaCommand => new Command(async () =>
		{
			try
			{
				CodigoInt = int.Parse(Codigo);
			}
			catch
			{
				CodigoInt = 0;
			}

			if (string.IsNullOrEmpty(Descripcion))
			{
				await MostrarAlerta("Error", "Debes agregar una descipcion a la incidencia");
				return;
			}

			var url = $"{baseUrl}/incidencias";

			var incidencia = new IncidenciaModel
			{
				created = DateTime.Now,
				createdByUser = ReportadaPor,
				code = CodigoInt,
				descripcion = Descripcion,
				isCompleted = false,
				canWorkNow = true,
				contractor = EsPrioridad,
				closedBy = Comentario
			};

			string json = JsonSerializer.Serialize<IncidenciaModel>(incidencia, serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PostAsync(url, content);

			if (response.IsSuccessStatusCode)
			{
				await MostrarAlerta("Anotado!", "Incidencia Creada Con Exito");
				LimpiarCampos();
			}
			else
			{
				await MostrarAlerta("Error", "Lo siento, hubo un error al agregar el registro");
			}

		});

		private async Task MostrarAlerta(string titulo, string mensaje)
		{
			await Application.Current.MainPage.DisplayAlert(titulo, mensaje, "Aceptar");
		}

		private void LimpiarCampos()
		{
			Codigo = "";
			CodigoInt = 0;
			Descripcion = "";
			ReportadaPor = "";
			EsPrioridad = false;
			Comentario = "";
		}
	}
}
