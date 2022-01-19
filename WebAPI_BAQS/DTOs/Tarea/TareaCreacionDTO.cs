using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.Tarea
{
    public class TareaCreacionDTO
    {
        //Editar
        public int IdCabecera { get; set; }
        public string Filtro { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        //la de inicio
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int Intervalo { get; set; }

    }
}
