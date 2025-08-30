namespace TaskTracker.Application.DTOs.Common
{
	public class PagedResponse<T>
	{
		public List<T> Items { get; set; } = new();
		public int Page { get; set; } = 0;			// Номер страницы.

		public int TotalCount { get; set; } = 0;	// Общее количество записей.
		public int PageSize { get; set; } = 0;		// Кол-во записей на странице
		public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);	//Кол-во страниц
	}
}
