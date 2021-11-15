using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Controllers
{
    [Route("api/companias")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CompaniaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Compania>>> Get()
        {
            return await context.Companias.ToListAsync();
        }
    }
}
