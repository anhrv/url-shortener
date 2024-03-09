using UrlShortener.Core.Domain.Models;

namespace UrlShortener.Core.Domain.RepositoryContracts
{
	public interface IUrlRepository
	{
		Task<UrlObject> AddUrlObject(UrlObject urlObject);
		Task<UrlObject?> GetUrlObjectByLong(string longUrl);
		Task<UrlObject?> GetUrlObjectByShort(string key);
		Task DeleteUrlObject (UrlObject urlObject);
		Task UseOneClick(UrlObject urlObject);
	}
}
