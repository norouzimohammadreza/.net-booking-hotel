using System.Net;
using System.Text.Json;

namespace BookingHotel.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteResponse(
                context,
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                "User is not authenticated.");
        }
        catch (Exception)
        {
            await WriteResponse(
                context,
                HttpStatusCode.InternalServerError,
                "ServerError",
                "Something went wrong.");
        }
    }

    private static async Task WriteResponse(
        HttpContext context,
        HttpStatusCode statusCode,
        string code,
        string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            Code = code,
            Message = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}