using Incidencias.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Incidencias.MVVM.ViewModels
{
	[AddINotifyPropertyChangedInterface]
	public class LoginViewModel
	{
		private readonly LoginService loginService;

		public string UserName { get; set; } = "";
		public string Password { get; set; } = "";

        public LoginViewModel(LoginService loginService)
        {
			this.loginService = loginService;
		}

		public ICommand LoginCommand => new Command(async () =>
		{
			await LoginMethod();
		});

		public async Task LoginMethod()
		{
			var result = await loginService.Login(UserName, Password);

			if (result)
			{
				Application.Current.MainPage = new AppShell();
			} else
			{
				await Application.Current.MainPage.DisplayAlert("Mensaje", "Credenciales Incorrectas", "Aceptar");
			}
		}
    }
}
