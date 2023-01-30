using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace AuthService
{
    public class WebsiteExceptionFilter : IExceptionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public WebsiteExceptionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void OnException(ExceptionContext context)
        {
            var e = context.Exception;

            switch (e)
            {
                case HttpRequestException { StatusCode: HttpStatusCode.Unauthorized }:
                    Log.Error($"Error 401 (Unauthorized): {context.HttpContext.Request.GetDisplayUrl()}");

                    context.Result = new UnauthorizedResult();

                    return;
            }

            Log.Error(e, e.GetBaseException().Message);

            context.Result = new BadRequestObjectResult(new
            {
                type = e.GetType().Name,
                message = e.Message,
                source = e.Source,
                targetSite = e.TargetSite?.Name
            });
        }
    }
}