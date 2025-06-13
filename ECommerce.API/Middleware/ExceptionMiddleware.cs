public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionMiddleware> _logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
      // Fix for CA2254: Use a constant message template and pass exception details as parameters
      const string logMessageTemplate = "Unhandled exception occurred: {ExceptionMessage}";
      _logger.LogError(logMessageTemplate, ex.Message);

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = ex switch
      {
        KeyNotFoundException => StatusCodes.Status404NotFound,
        ArgumentException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
      };

      var response = new { message = ex.Message, status = context.Response.StatusCode };
      await context.Response.WriteAsJsonAsync(response);
    }
  }
}