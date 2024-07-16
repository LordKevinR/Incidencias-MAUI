using Incidencias.MVVM;

namespace Incidencias
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			BindingContext = new PrincipalViewModel();
		}
	}
}
