using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.Tarea
{
    public class TareaEditarDTO
    {
        public int IdTarea { get; set; }
        public int IdCabecera { get; set; }
        public string Nombre { get; set; }
        public string Filtro { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int Intervalo { get; set; }
    }
}
