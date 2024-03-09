using UrlShortener.Core.Domain.Models;
using UrlShortener.Core.DTO;

namespace UrlShortener.Core.ServiceContracts
{
	public interface IUrlService
	{
		Task<UrlObject> Shorten(ShortenUrlRequest request);
		Task<UrlObject?> GetUrlObjectByLong(string longUrl);
		Task<UrlObject?> GetUrlObjectByShort(string key);
		Task DeleteUrlObject(UrlObject urlObject);
		Task UseOneClick(UrlObject urlObject);
		bool CheckIfValidUrl(string url);
	}
}
