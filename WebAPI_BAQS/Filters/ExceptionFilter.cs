using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_BAQS.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<Exception> logger;

        public ExceptionFilter(ILogger<Exception> logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            //Procesa cualaquier error
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);

            //Registrarlo com un filtro global en el startup
        }
    }
}
