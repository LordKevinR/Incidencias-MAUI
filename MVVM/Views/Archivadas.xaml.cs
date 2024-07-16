using Incidencias.MVVM.ViewModels;
using Incidencias.Services;

namespace Incidencias.MVVM.Views;

public partial class Archivadas : ContentPage
{
	public Archivadas()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}
}