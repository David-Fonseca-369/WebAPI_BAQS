using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs;

namespace WebAPI_BAQS.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina)//omite un número específico de elementos en una secuencia y luego devuelve los elementos restantes
                .Take(paginacionDTO.RecordsPorPagina);//devuelve un número específico de elementos continuos desde el inicio de una secuencia
        }
    }
}
