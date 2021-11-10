using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UsuariosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        //GET: api/usuarios/getUsuarios
        [HttpGet("getUsuarios")]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            return await context.Usuarios.ToListAsync();
        }

        //POST: api/usuarios/insertar
        [HttpPost("insertar")]
        public async Task<ActionResult> Insertar([FromBody] UsuarioDTO usuarioDTO)
        {
            Usuario usuario = new Usuario
            {
                IdRol = usuarioDTO.IdRol,
                IdCompania = usuarioDTO.IdCompania,
                Nombre = usuarioDTO.Nombre,
                Email = usuarioDTO.Email,
                _Password = usuarioDTO._Password,
                Estado = true //Por default sea true
            };

            //Agegrar al tabla usuario
            context.Add(usuario);

            //Guardar cambios
            await context.SaveChangesAsync();

            return Ok();
        }

        //editar usuario
        [HttpPut("modificar")]
        public async Task<ActionResult> Modificar([FromBody] UsuarioActualizarDTO usuarioActualizarDTO)
        {
            if (usuarioActualizarDTO.IdUsuario > 0) //si hay un id
            {
                var usuario = await context.Usuarios.Where(x => x.IdUsuario == usuarioActualizarDTO.IdUsuario).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    usuario.IdRol = usuarioActualizarDTO.IdRol;
                    usuario.IdCompania = usuarioActualizarDTO.IdCompania;
                    usuario.Nombre = usuarioActualizarDTO.Nombre;
                    usuario.Email = usuarioActualizarDTO.Email;
                    usuario._Password = usuarioActualizarDTO._Password;
                    usuario.Estado = usuarioActualizarDTO.Estado;


                    await context.SaveChangesAsync();

                    return Ok();
                }


                return NotFound("Usuario no encontrado.");

            }

            return BadRequest();

        }



        //Desactivar
        //PUT: api/usuarios/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar([FromRoute]int id)
        {
            if (id > 0)
            {
                var usuario = await context.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    usuario.Estado = false;

                    await context.SaveChangesAsync();

                    return Ok();
                }

                return NotFound(new { mensaje = "Usuario no encontrado." });

            }

            return BadRequest();
        }



        //Activar
        //PUT: api/usuarios/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar([FromRoute]int id)
        {
            if (id > 0)
            {
                var usuario = await context.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    usuario.Estado = true;

                    await context.SaveChangesAsync();

                    return Ok();
                }

                return NotFound(new { mensaje = "Usuario no encontrado." });

            }

            return BadRequest();
        }

    }
}
