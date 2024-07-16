using Incidencias.MVVM.ViewModels;

namespace Incidencias.MVVM.Views;

public partial class IncidenciasPrioridades : ContentPage
{
	public IncidenciasPrioridades()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}
}