using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs;
using WebAPI_BAQS.DTOs.Area;
using WebAPI_BAQS.Entities;
using WebAPI_BAQS.Helpers;

namespace WebAPI_BAQS.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AreasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        //GET: api/areas/todas
        [HttpGet("todas")]
        public async Task<ActionResult<List<AreaDTO>>> Todas()
        {
            //Areas para select por lo que solo seran visibles las habilitadas.
            var areas = await context.Areas.Where(x => x.Estado).ToListAsync();
            return mapper.Map<List<AreaDTO>>(areas);
        }

        //GET: api/areas/TodasPaginacion
        [HttpGet("todasPaginacion")]
        public async Task<ActionResult<List<AreaDTO>>> TodasPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Areas.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var areasPaginacion = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<AreaDTO>>(areasPaginacion);

        }

        //GET: api/areas/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AreaDTO>>Get([FromRoute]int id)
        {

            var area = await context.Areas.FirstOrDefaultAsync(x => x.IdArea == id);

            if (area == null)
            {
                return NotFound();
            }

            return mapper.Map<AreaDTO>(area);

        }


        //POST: api/areas/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] AreaCreacionDTO areaCreacionDTO)
        {
            var area = mapper.Map<Area>(areaCreacionDTO);
            area.Estado = true;

            context.Add(area);
            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/areas/editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar([FromRoute] int id, [FromBody] AreaCreacionDTO areaCreacionDTO)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.IdArea == id);

            if (area == null)
            {
                return NotFound();
            }

            area.Nombre = areaCreacionDTO.Nombre;

            await context.SaveChangesAsync();

            return NoContent();
        }


        //PUT: api/areas/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult>Desactivar(int id)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.IdArea == id);

            if (area == null)
            {
                return NotFound();
            }

            area.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }
        
        //PUT: api/areas/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult>Activar(int id)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.IdArea == id);

            if (area == null)
            {
                return NotFound();
            }

            area.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }

             



        


    }
}
