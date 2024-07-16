using Incidencias.MVVM.Views;

namespace Incidencias
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//var token = Preferences.Get("token", string.Empty);
			//if(string.IsNullOrEmpty(token))
			//{
			//	MainPage = loginView;
			//}
			//else
			//{
			//}
				MainPage = new AppShell();
		}
	}
}
