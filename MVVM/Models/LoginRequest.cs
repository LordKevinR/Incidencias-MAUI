using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidencias.MVVM.Models
{
	internal class LoginRequest
	{
		public string UserName { get; set; } = "";
		public string Password { get; set; } = "";
    }
}
