using UrlShortener.Core.Domain.Models;

namespace UrlShortener.Core.Domain.RepositoryContracts
{
	public interface ICacheRepository
	{
		Task AddUrlObjectsAsync(UrlObject urlObject, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
		Task<UrlObject?> GetUrlObjectByShortAsync(string key);
		Task<UrlObject?> GetUrlObjectByLongAsync(string longUrl);
		Task DeleteUrlObjectsAsync(UrlObject urlObject);
		Task UseOneClickAsync(UrlObject urlObject);
	}
}
