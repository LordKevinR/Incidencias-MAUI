using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidencias.Services
{
	public interface IIncidenciasServices
	{
		Task GetAllIncidencias();
		Task RefreshCommand();
	}
}
