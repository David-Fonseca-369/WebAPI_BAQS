using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public int IdCompania { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Estado { get; set; }

        //Rerences 
        public Rol Rol { get; set; }
        public Compania Compania { get; set; }
        

    }
}
