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
        public async Task<ActionResult<List<UsuarioVistaDTO>>> GetUsuarios()
        {
            var usuarios = await context.Usuarios.Include(x => x.Rol).Include(x => x.Compania).ToListAsync();

            return usuarios.Select(x => new UsuarioVistaDTO
            {
                IdUsuario = x.IdUsuario,
                IdRol = x.IdRol,
                NombreRol = x.Rol.Nombre,
                IdCompania = x.IdCompania,
                NombreCompania = x.Compania.Nombre,
                Nombre = x.Nombre,
                Email = x.Email,
                Estado = x.Estado
            }).ToList();
        }

        //GET: api/usuarios/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioVistaDTO>> Get(int id)
        {
            var usuario = await context.Usuarios.Include(x => x.Rol).Include(x => x.Compania).FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }


            return new UsuarioVistaDTO
            {
                IdUsuario = usuario.IdUsuario,
                IdRol = usuario.IdRol,
                NombreRol = usuario.Rol.Nombre,
                IdCompania = usuario.IdCompania,
                NombreCompania = usuario.Compania.Nombre,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Estado = usuario.Estado
            };

        }



        //POST: api/usuarios/insertar
        [HttpPost("insertar")]
        public async Task<ActionResult> Insertar([FromBody] UsuarioDTO usuarioDTO)
        {

            if (await VerificarEmail(usuarioDTO.Email))
            {
                return BadRequest("El email ya existe.");
            }

            Usuario usuario = new Usuario
            {
                IdRol = usuarioDTO.IdRol,
                IdCompania = usuarioDTO.IdCompania,
                Nombre = usuarioDTO.Nombre,
                Email = usuarioDTO.Email.ToLower(),
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
        [HttpPut("modificar/{id:int}")]
        public async Task<ActionResult> Modificar(int id, [FromBody] UsuarioActualizarDTO usuarioActualizarDTO)
        {
            if (id > 0) //si hay un id
            {
                var usuario = await context.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefaultAsync();

                if (usuarioActualizarDTO.Email.ToLower() != usuario.Email)
                {
                    if (await VerificarEmail(usuarioActualizarDTO.Email))
                    {
                        return BadRequest("El email ya existe.");
                    }
                }



                if (usuario != null)
                {
                    usuario.IdRol = usuarioActualizarDTO.IdRol;
                    usuario.IdCompania = usuarioActualizarDTO.IdCompania;
                    usuario.Nombre = usuarioActualizarDTO.Nombre;
                    usuario.Email = usuarioActualizarDTO.Email.ToLower();

                    if (!string.IsNullOrEmpty(usuarioActualizarDTO._Password))
                    {
                        usuario._Password = usuarioActualizarDTO._Password;
                    }

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
        public async Task<ActionResult> Desactivar([FromRoute] int id)
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
        public async Task<ActionResult> Activar([FromRoute] int id)
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


        private async Task<bool> VerificarEmail(string email)
        {
            email = email.ToLower();
            return await context.Usuarios.AnyAsync(x => x.Email == email);
        }

    }
}
