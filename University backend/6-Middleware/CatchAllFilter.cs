using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace University_backend;

public class CatchAllFilter : IExceptionFilter
{
    private readonly ILogger<CatchAllFilter> _logger;

    public CatchAllFilter(ILogger<CatchAllFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred: {Message}", context.Exception.Message);

        string message = AppConfig.IsProduction
            ? "Some error occurred, please try again."
            : GetInnerMessage(context.Exception);

        InternalServerError error = new InternalServerError(message);
        JsonResult result = new JsonResult(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.Result = result;
        context.ExceptionHandled = true;
    }

    private string GetInnerMessage(Exception ex)
    {
        if (ex == null) return "";
        if (ex.InnerException == null) return ex.Message;
        return GetInnerMessage(ex.InnerException);
    }
}
