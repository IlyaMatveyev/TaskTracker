namespace TaskTracker.Application.Interfaces
{
	public interface ICacheService
	{
		Task<T> GetCachedValue<T>(
			string cacheKey, 
			Func<Task<T>> factory,
			TimeSpan? slidingExpiration = null,
			TimeSpan? absoluteExpiration = null);
	}
}
