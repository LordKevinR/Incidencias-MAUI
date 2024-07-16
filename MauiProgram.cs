using Incidencias.MVVM.ViewModels;
using Incidencias.MVVM.Views;
using Incidencias.Services;
using Microsoft.Extensions.Logging;

namespace Incidencias
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddSingleton<IIncidenciasServices, IncidenciasServices>();
			builder.Services.AddTransient<MainViewModel>();
			builder.Services.AddTransient<Archivadas>();
			builder.Services.AddTransient<CrearIncidencia>();
			builder.Services.AddTransient<MainView>();
			builder.Services.AddTransient<IncidenciasCompletasView>();
			builder.Services.AddTransient<IncidenciasPrioridades>();

			builder.Services.AddTransient<HttpClient>();
			builder.Services.AddSingleton<LoginService>();
			builder.Services.AddTransient<LoginView>();
			builder.Services.AddTransient<LoginViewModel>();


#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
