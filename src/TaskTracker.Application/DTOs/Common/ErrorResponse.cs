using System.Web.Http.Results;

namespace TaskTracker.Application.DTOs.Common
{
	/// <summary>
	/// Информация об ошибке.
	/// </summary>
	public class ErrorResponse
	{
		/// <summary>
		/// Статус код.
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// Заголовок.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Сообщение об исключении.
		/// </summary>
		public string ExceptionMessage { get; set; }
	}
}
