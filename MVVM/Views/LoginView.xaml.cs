using Incidencias.MVVM.ViewModels;

namespace Incidencias.MVVM.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel;
	}
}