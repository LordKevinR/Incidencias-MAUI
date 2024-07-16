using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidencias.MVVM.Models
{
	internal class LoginResponse
	{
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string LasName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Token { get; set; } = "";
    }
}
