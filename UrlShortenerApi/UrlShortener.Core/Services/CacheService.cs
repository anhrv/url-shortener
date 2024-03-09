using UrlShortener.Core.Domain.Models;
using UrlShortener.Core.Domain.RepositoryContracts;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortener.Core.Services
{
	public class CacheService : ICacheService
	{
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
			_cacheRepository = cacheRepository;
        }
        public async Task AddUrlObjectsAsync(UrlObject urlObject,
                                        TimeSpan? absoluteExpireTime = null,
                                        TimeSpan? unusedExpireTime = null)
        {
            await _cacheRepository.AddUrlObjectsAsync(urlObject, absoluteExpireTime, unusedExpireTime);
		}

        public async Task<UrlObject?> GetUrlObjectByShortAsync(string key)
        {
            return await _cacheRepository.GetUrlObjectByShortAsync(key);
        }
		public async Task<UrlObject?> GetUrlObjectByLongAsync(string longUrl)
		{
			return await _cacheRepository.GetUrlObjectByShortAsync(longUrl);
		}

		public async Task DeleteUrlObjectsAsync(UrlObject urlObject)
        {
            await _cacheRepository.DeleteUrlObjectsAsync(urlObject);
		}

        public async Task UseOneClickAsync(UrlObject urlObject)
        {
			await _cacheRepository.UseOneClickAsync(urlObject);
		}
	}
}
