using UrlShortener.Core.Domain.Models;
using UrlShortener.Core.Domain.RepositoryContracts;
using UrlShortener.Core.DTO;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortener.Core.Services
{
	public class UrlService : IUrlService
	{
		private readonly IUrlRepository _urlRepository;
		private readonly ICacheService _cacheService;

        public UrlService(IUrlRepository urlRepository, ICacheService cacheService)
        {
			_urlRepository = urlRepository;
			_cacheService = cacheService;
        }

		public async Task<UrlObject> Shorten(ShortenUrlRequest request)
		{
			var key = string.Empty;
			var existingKey = new UrlObject();
			var found = false;

			do
			{
				key = this.GenerateKey();
				existingKey = await _cacheService.GetUrlObjectByShortAsync(key);
				if(existingKey == null)
				{
					existingKey = await _urlRepository.GetUrlObjectByShort(key);
					if (existingKey != null)
					{
						found = true;
					}
				}
			} while (found);

			var urlObject = new UrlObject
			{
				LongUrl = request.LongUrl,
				Key = key,
				RemainingClicks = request.RemainingClicks,
				ExpirationDate = request.ExpiresInMinutes == null ? null : DateTime.UtcNow.AddMinutes(Convert.ToDouble(request.ExpiresInMinutes)),
			};

			var newUrlObject = await _urlRepository.AddUrlObject(urlObject);
			await _cacheService.AddUrlObjectsAsync(newUrlObject);
			return newUrlObject;
		}

		public async Task<UrlObject?> GetUrlObjectByLong(string longUrl)
		{
			var urlObject = await _cacheService.GetUrlObjectByLongAsync(longUrl);
			if(urlObject == null)
			{
				urlObject = await _urlRepository.GetUrlObjectByLong(longUrl);
				if (urlObject != null)
					await _cacheService.AddUrlObjectsAsync(urlObject);
			}
			return urlObject;
		}

		public async Task<UrlObject?> GetUrlObjectByShort(string key)
		{
			var urlObject = await _cacheService.GetUrlObjectByShortAsync(key);
			if (urlObject == null)
			{
				urlObject = await _urlRepository.GetUrlObjectByShort(key);
				if (urlObject != null)
					await _cacheService.AddUrlObjectsAsync(urlObject);
			}
			return urlObject;
		}

		public async Task DeleteUrlObject(UrlObject urlObject)
		{
			await _cacheService.DeleteUrlObjectsAsync(urlObject);
			await _urlRepository.DeleteUrlObject(urlObject);
			return;
		}

		public async Task UseOneClick(UrlObject urlObject)
		{
			await _cacheService.UseOneClickAsync(urlObject);
			await _urlRepository.UseOneClick(urlObject);
			return;
		}

		public bool CheckIfValidUrl(string url)
		{
			return Uri.TryCreate(url, UriKind.Absolute, out _) && !string.IsNullOrWhiteSpace(url);
		}

		private string GenerateKey(int lenght = 6)
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[lenght];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);

			return finalString;
		}
	}
}
