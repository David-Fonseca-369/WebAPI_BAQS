using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.BAQ
{
    public class BAQDTO
    {
        public int IdCabecera { get; set; }
        public int IdArea { get; set; }
        public string NombreArea { get; set; }
        public string Nombre { get; set; }
        public string _url { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }

    }
}
