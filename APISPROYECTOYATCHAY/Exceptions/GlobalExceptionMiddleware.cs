namespace APISPROYECTOYATCHAY.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no manejada");
                await ManejarExcepcionAsync(context, ex);
            }
        }

        private static Task ManejarExcepcionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var respuesta = new
            {
                exito = 0,
                mensaje = exception.Message,
                detalles = exception.GetType().Name
            };

            // Códigos específicos según tipo de excepción
            context.Response.StatusCode = exception switch
            {
                InvalidOperationException => StatusCodes.Status400BadRequest,
                DecisionAlreadyMadeException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            return context.Response.WriteAsJsonAsync(respuesta);
        }
    }
}
