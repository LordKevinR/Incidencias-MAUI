using Incidencias.MVVM.ViewModels;

namespace Incidencias.MVVM.Views;

public partial class IncidenciasCompletasView : ContentPage
{
	public IncidenciasCompletasView()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}
}