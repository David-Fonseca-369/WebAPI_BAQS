using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs;
using WebAPI_BAQS.DTOs.BAQ;
using WebAPI_BAQS.Entities;
using WebAPI_BAQS.Helpers;

namespace WebAPI_BAQS.Controllers
{
    [Route("api/baqs")]
    [ApiController]
    public class BaqsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public BaqsController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet("baqsPaginacion")]
        public async Task<ActionResult<List<BAQDTO>>> BaqsPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var baqs = await context.BaqCabeceras.Include(x => x.Area).ToListAsync();

            int cantidad = baqs.Count;


            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = baqs.AsQueryable();

            var baqsPaginacion = queryable.Paginar(paginacionDTO).ToList();

            //var baqsMap = mapper.Map<List<BAQDTO>>(baqsPaginacion);

            return baqsPaginacion.Select(x => new BAQDTO
            {
                IdCabecera = x.IdArea,
                IdArea = x.IdArea,
                NombreArea = x.Area.Nombre,
                Nombre = x.Nombre,
                _url = x._url,
                Fecha = x.Fecha,
                Estado = x.Estado
            }).ToList();

        }



        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] BaqCreacionDTO baqCreacionDTO)
        {
            //Crear la query para generar tabla en insertar a la base datos
            var result = await CreateTableBAQ(baqCreacionDTO);

            //Si es correcta, entonces almacenar los registros de la query en la BD
            if (result)
            {
                //
                BaqCabecera cabecera = new()
                {
                    IdArea = baqCreacionDTO.IdArea,
                    Nombre = baqCreacionDTO.Nombre,
                    _url = baqCreacionDTO._url,
                    Fecha = DateTime.Now,
                    Estado = true
                };

                context.Add(cabecera);

                await context.SaveChangesAsync();

                if (baqCreacionDTO.Columnas.Count > 0)
                {
                    foreach (var columna in baqCreacionDTO.Columnas)
                    {

                        BaqDetalle columnaCreacion = new()
                        {
                            IdCabecera = cabecera.IdCabecera,
                            NombreColumna = columna.NombreColumna,
                            Tipo = columna.Tipo,
                            Longitud = columna.Longitud == null ? 0 : Convert.ToInt32(columna.Longitud),
                            Flotantes = columna.Flotantes == null ? 0 : Convert.ToInt32(columna.Flotantes),
                            Nulos = columna.Nulos
                        };

                        context.Add(columnaCreacion);

                    }
                    await context.SaveChangesAsync();
                }

                return NoContent();
            }


            return BadRequest($"Error al crear la tabla {baqCreacionDTO.Nombre}");
        }



        private async Task<bool> CreateTableBAQ(BaqCreacionDTO baqCreacionDTO)
        {

            string columnas = "";

            foreach (var columna in baqCreacionDTO.Columnas)
            {
                string nulos = columna.Nulos ? "NULL" : "NOT NULL";

                if (columna.Tipo == "varchar")
                {
                    columnas += $" {columna.NombreColumna}  varchar({columna.Longitud}) {nulos},";
                }

                if (columna.Tipo == "int" || columna.Tipo == "bit" || columna.Tipo == "date" || columna.Tipo == "datetime")
                {

                    columnas += $"{columna.NombreColumna} {columna.Tipo} {nulos},";
                }

                if (columna.Tipo == "decimal")
                {
                    columnas += $"{columna.NombreColumna} decimal ({columna.Longitud}, {columna.Flotantes}) {nulos},";
                }
            }

            string table = $"CREATE TABLE {baqCreacionDTO.Nombre}({columnas});";

            var result = await ExecuteQuery(table);

            return result;

        }

        private async Task<bool> ExecuteQuery(string query)
        {
            string connectionString = configuration.GetValue<string>("connectionDB");
            var sqlConnection = new SqlConnection(connectionString);
            var command = new SqlCommand(query, sqlConnection);
            try
            {
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await command.Connection.CloseAsync();

                return true;
            }
            catch (Exception ex)
            {
                await command.Connection.CloseAsync();

                //Envio correo
                return false;
            }

        }


    }
}
