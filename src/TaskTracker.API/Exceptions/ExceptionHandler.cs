using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Application.DTOs.Common;

namespace TaskTracker.API.Exceptions
{
	public class ExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<ExceptionHandler> _logger;

		public ExceptionHandler(ILogger<ExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext, 
			Exception exception, 
			CancellationToken cancellationToken)
		{
			_logger.LogError(exception.Message);

			var (statusCode, title) = MapException(exception);

			var errorResponse = new ErrorResponse()
			{
				StatusCode = statusCode,
				Title = title,
				ExceptionMessage = exception.Message
			};

			httpContext.Response.StatusCode = errorResponse.StatusCode;
			await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

			return true;
		}

		private static (int StatusCode, string Title) MapException(Exception exception)
		{
			return exception switch
			{
				KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
				ValidationException => (StatusCodes.Status400BadRequest, "Bad Request"),
				_ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
			};
		}
	}
}
