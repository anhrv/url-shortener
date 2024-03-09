using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UrlShortener.Core.Domain.Models;
using UrlShortener.Core.Domain.RepositoryContracts;

namespace UrlShortener.Infrastructure.Repositories
{
	public class CacheRepository : ICacheRepository
	{
		private readonly IDistributedCache _cache;

		public CacheRepository(IDistributedCache cache)
		{
			_cache = cache;
		}

		public async Task AddUrlObjectsAsync(UrlObject urlObject, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
		{
			var options = new DistributedCacheEntryOptions();

			options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
			options.SlidingExpiration = unusedExpireTime;

			var urlObjectJson = JsonSerializer.Serialize(urlObject);
			await _cache.SetStringAsync(urlObject.Key, urlObjectJson, options);
			await _cache.SetStringAsync(urlObject.LongUrl, urlObjectJson, options);
		}

		public async Task<UrlObject?> GetUrlObjectByLongAsync(string longUrl)
		{
			var urlObjectJson = await _cache.GetStringAsync(longUrl);

			if (urlObjectJson == null)
				return null;

			var urlObject = JsonSerializer.Deserialize<UrlObject>(urlObjectJson);

			if (urlObject != null && urlObject.RemainingClicks == null && urlObject.ExpirationDate == null)
				return urlObject;

			return null;
		}

		public async Task<UrlObject?> GetUrlObjectByShortAsync(string key)
		{
			var urlObjectJson = await _cache.GetStringAsync(key);

			if (urlObjectJson == null)
				return null;

			return JsonSerializer.Deserialize<UrlObject>(urlObjectJson);
		}

		public async Task DeleteUrlObjectsAsync(UrlObject urlObject)
		{
			var urlObjectByShort = await _cache.GetStringAsync(urlObject.Key);
			var urlObjectByLong = await _cache.GetStringAsync(urlObject.LongUrl);
			if (urlObjectByShort != null && urlObjectByLong != null)
			{
				await _cache.RemoveAsync(urlObject.Key);
				await _cache.RemoveAsync(urlObject.LongUrl);
			}
		}

		public async Task UseOneClickAsync(UrlObject urlObject)
		{
			var urlObjectByShortJson = await _cache.GetStringAsync(urlObject.Key);
			var urlObjectByLongJson = await _cache.GetStringAsync(urlObject.LongUrl);
			if (urlObjectByShortJson != null && urlObjectByLongJson != null)
			{
				var urlObjectByShort = JsonSerializer.Deserialize<UrlObject>(urlObjectByShortJson);
				urlObjectByShort!.RemainingClicks--;
				if (urlObjectByShort.RemainingClicks == 0)
				{
					await _cache.RemoveAsync(urlObject.Key);
					await _cache.RemoveAsync(urlObject.LongUrl);
				}
				else
				{
					await AddUrlObjectsAsync(urlObjectByShort);
				}
			}
		}
	}
}
