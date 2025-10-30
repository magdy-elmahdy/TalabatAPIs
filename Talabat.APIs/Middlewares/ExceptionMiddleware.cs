using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env)
        {
            _next = next;    // ال ميدلوير اللي بعديها
            _logger = logger; //
            _env = env; // بيئة التشغيل
        }
        public async Task InvokeAsync(HttpContext httpContext) //ده ال ريكويست 
        {

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.StackTrace.ToString());
                //Header Erorr 
                httpContext.Response.StatusCode =(int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                //Body Error
                var response = _env.IsDevelopment ()?
                                new ApiExceptionResponse(500 , ex.Message , ex.StackTrace.ToString())
                                :new ApiExceptionResponse(500 , "Internal Server Error");
                var Json = JsonSerializer.Serialize(response);

                await httpContext.Response.WriteAsync(Json);
            }

            

        }
    }
}
