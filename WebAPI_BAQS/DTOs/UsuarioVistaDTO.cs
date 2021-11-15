using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.DTOs
{
    public class UsuarioVistaDTO
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public int IdCompania { get; set; }
        public string NombreCompania { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }
    }
}
