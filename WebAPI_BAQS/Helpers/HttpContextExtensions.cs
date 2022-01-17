using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            //cuenta la cantidad de registro de una tabla
            double cantidad = await queryable.CountAsync();

            //lo agrego a la cabecera
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
            //debemos exponer esta cabecera em el startup, ya que no todas las cabeceras se pueden ver.
        }

        public static void InsertarParametrosPaginacionEnCabeceraPersonalizado(this HttpContext httpContext, int cantidad)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            //lo agrego a la cabecera
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());
            //debemos exponer esta cabecera em el startup, ya que no todas las cabeceras se pueden ver.

        }
    }
}
