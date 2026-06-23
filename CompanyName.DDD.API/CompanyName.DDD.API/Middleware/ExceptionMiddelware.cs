using CompanyName.DDD.Domain.Common.Log;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Text.Json;
namespace CompanyName.DDD.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _log;
        private readonly string _environment;
        private readonly bool _isDevelopment;

        public ExceptionMiddleware(RequestDelegate next, ILog log)
        {
            _next = next;
            _log = log;
            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            _isDevelopment = _environment.Equals("Local");
        }

        private Task HandleGlobalExceptionAsync(HttpContext context, int statusCode, string message, Exception? exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var error = _isDevelopment ? exception?.Message : "An error occurred";
            var innerError = _isDevelopment ? exception?.InnerException?.Message : null;

            var result = new { message, error, innerError };
            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Info", $"Llamada a controlador {context.Request.Path} correcta", ""));
            }
            catch (OperationCanceledException)
            {
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Warning", "La solicitud fue cancelada", ""));
            }
            catch (SqlException ex)
            {
                HandleSqlException(context, ex);
            }
            catch (DbUpdateException ex)
            {
                var customMessage = "Se ha generado un error de consistencia en la base de datos";
                await HandleGlobalExceptionAsync(context, 500, customMessage, ex);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", customMessage, new { message = ex.Message, innerException = ex.InnerException?.Message }));
            }
            catch (ArgumentException ex)
            {
                var customMessage = "Parámetros inválidos en la solicitud";
                await HandleGlobalExceptionAsync(context, 400, customMessage, ex);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", customMessage, new { message = ex.Message }));
            }
            catch (SecurityTokenInvalidLifetimeException ex)
            {
                var customMessage = "El token de autenticación ha expirado";
                await HandleGlobalExceptionAsync(context, 401, customMessage, ex);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", customMessage, new { message = ex.Message }));
            }
            catch (SecurityTokenException ex)
            {
                var customMessage = "Token de autenticación inválido";
                await HandleGlobalExceptionAsync(context, 401, customMessage, ex);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", customMessage, new { message = ex.Message }));
            }
            catch (WarningException ex)
            {
                await HandleGlobalExceptionAsync(context, 400, ex.Message, ex);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", ex.Message, new { message = ex.Message }));
            }
            catch (Exception ex)
            {
                var customMessage = "Ha ocurrido un error no controlado con la aplicación";
                await HandleGlobalExceptionAsync(context, 500, customMessage, ex);
                _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", customMessage, new { message = ex.Message, innerException = ex.InnerException?.Message }));
            }
        }

        private async Task HandleSqlException(HttpContext context, SqlException ex)
        {
            string customMessage;
            int statusCode;

            if (ex.Number == 53)
            {
                customMessage = "Error al conectar con la base de datos";
                statusCode = 503;
            }
            else if (ex.Number == -2)
            {
                customMessage = "Se ha superado el tiempo de espera con la base de datos";
                statusCode = 408;
            }
            else
            {
                customMessage = "Ha ocurrido un error no controlado con la base de datos";
                statusCode = 500;
            }

            await HandleGlobalExceptionAsync(context, statusCode, customMessage, ex);
            _log.Send(new BodyLog(Guid.NewGuid(), _environment, "Error", customMessage, new { message = ex.Message, innerException = ex.InnerException?.Message }));
        }
    }
}