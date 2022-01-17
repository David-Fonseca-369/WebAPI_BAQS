using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.Entities
{
    public class BaqCabecera
    {
        public int IdCabecera { get; set; }
        [Required]
        public int IdArea { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string _url { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }

        //Realciones
        public Area Area { get; set; }

    }
}
