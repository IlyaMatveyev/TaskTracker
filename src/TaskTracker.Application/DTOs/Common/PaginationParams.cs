namespace TaskTracker.Application.DTOs.Common
{
	public class PaginationParams
	{
		public const int MAX_PAGE_SIZE = 100;

		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 1;

		public PaginationParams(int page, int pageSize)
		{
			if(page < 1 || 
				page > MAX_PAGE_SIZE || 
				pageSize < 0)
			{
				throw new ArgumentException("Invalid pagination params.");
			}

			Page = page;
			PageSize = pageSize;
		}
	}
}
