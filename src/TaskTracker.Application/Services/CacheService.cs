using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.Application.Services
{
	public class CacheService : ICacheService
	{
		private readonly ILogger<CacheService> _logger;
		private readonly IMemoryCache _memoryCache;

		public CacheService(
			ILogger<CacheService> logger, 
			IMemoryCache memoryCache)
		{
			_logger = logger;
			_memoryCache = memoryCache;
		}

		public async Task<T> GetCachedValue<T>(
			string cacheKey, 
			Func<Task<T>> factory, 
			TimeSpan? slidingExpiration = null, 
			TimeSpan? absoluteExpiration = null)
		{
			if (_memoryCache.TryGetValue(cacheKey, out T? value))
			{
				_logger.LogInformation($"Значения по ключу {cacheKey} найдены в MemoryCache.");

				return value;
			}

			_logger.LogInformation($"Значения по ключу {cacheKey} не найдены в MemoryCache.");

			value = await factory();

			var cacheEntryOptions = new MemoryCacheEntryOptions();

			if (slidingExpiration.HasValue)
			{
				cacheEntryOptions.SetSlidingExpiration(slidingExpiration.Value);
			}

			if (absoluteExpiration.HasValue)
			{
				cacheEntryOptions.SetAbsoluteExpiration(absoluteExpiration.Value);
			}
				
			cacheEntryOptions.SetPriority(CacheItemPriority.Normal);

			_memoryCache.Set(cacheKey, value, cacheEntryOptions);

			return value;
		}
	}
}
