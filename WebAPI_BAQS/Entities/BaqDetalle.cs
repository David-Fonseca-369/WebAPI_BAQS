using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.Entities
{
    public class BaqDetalle
    {
        public int IdDetalle { get; set; }
        [Required]
        public int IdCabecera { get; set; }
        [Required]
        public string NombreColumna { get; set; }
        [Required]
        public string Tipo { get; set; }
        public int Longitud { get; set; }
        public int Flotantes { get; set; }
        [Required]
        public bool Nulos { get; set; }

        public BaqCabecera BaqCabecera { get; set; }
    }
}
