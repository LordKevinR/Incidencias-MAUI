using Incidencias.MVVM.ViewModels;

namespace Incidencias.MVVM.Views;

public partial class CrearIncidencia : ContentPage
{
	public CrearIncidencia()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}
}