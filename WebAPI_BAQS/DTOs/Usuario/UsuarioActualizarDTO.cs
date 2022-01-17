using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs.Usuario
{
    public class UsuarioActualizarDTO
    {
        [Required]
        public int IdRol { get; set; }
        [Required]
        public int IdCompania { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
