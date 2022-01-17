using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.Area
{
    public class AreaCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }
    }
}
