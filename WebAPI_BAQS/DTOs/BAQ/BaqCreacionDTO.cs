using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.BAQ
{
    public class BaqCreacionDTO
    {
        
        [Required]
        public int IdArea { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string _url { get; set; }

        //Columnas
        public List<ColumnasDTO> Columnas { get; set; }
    }
}
