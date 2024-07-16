using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incidencias.MVVM.Models
{
    public class IncidenciaModel
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public string createdByUser { get; set; }
        public int code { get; set; }
        public string descripcion { get; set; }
        public bool isCompleted { get; set; }
        public bool canWorkNow { get; set; }
        public bool contractor { get; set; }
        public string closedBy { get; set; }
    }

}
