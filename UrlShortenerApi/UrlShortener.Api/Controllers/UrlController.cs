using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.DTO;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortener.Api.Controllers
{
	public class UrlController : MyControllerBase
	{

		private readonly IUrlService _urlService;
		private readonly IConfiguration _config;

        public UrlController(IUrlService urlService, IConfiguration config)
        {
			_urlService = urlService;
			_config = config;
        }

        [HttpPost("/shorten")]
		public async Task<IActionResult> Shorten([FromBody] ShortenUrlRequest request)
		{
			if(!_urlService.CheckIfValidUrl(request.LongUrl))
			{
				return BadRequest(new ErrorResponse(400, "Bad Request","Something went wrong!<br>The provided URL is not a valid URL."));
			}

			if(request.RemainingClicks == null && request.ExpiresInMinutes == null)
			{
				var existingUrlObject = await _urlService.GetUrlObjectByLong(request.LongUrl);
				if (existingUrlObject != null)
				{
					return Ok(new ShortenUrlResponse { ShortUrl = $"https://{HttpContext.Request.Host}/{existingUrlObject.Key}" });
				}
			}

			var newUrlObject = await _urlService.Shorten(request);

			return Ok(new ShortenUrlResponse { ShortUrl = $"https://{HttpContext.Request.Host}/{newUrlObject.Key}" });
		}

		[HttpGet("/{key}")]
		public async Task<IActionResult> RedirectTo([FromRoute] string key)
		{
			var urlObject = await _urlService.GetUrlObjectByShort(key);

			if(urlObject == null)
			{
				//return NotFound(new ErrorResponse(404, "Not Found", "The shortened URL is either expired or not valid."));
				return Redirect(_config["ClientErrorPage"]!);
			}

			if(urlObject.ExpirationDate != null)
			{
				if(urlObject.ExpirationDate < DateTime.UtcNow)
				{
					await _urlService.DeleteUrlObject(urlObject);
					//return NotFound(new ErrorResponse(404, "Not Found", "The shortened URL is either expired or not valid."));
					return Redirect(_config["ClientErrorPage"]!);

				}
				HttpContext.Response.Headers.CacheControl = "no-cache, no-store";
			}

			var longUrl = urlObject.LongUrl;

			if (urlObject.RemainingClicks != null)
			{
				await _urlService.UseOneClick(urlObject);
				HttpContext.Response.Headers.CacheControl = "no-cache, no-store";
			}

			return RedirectPermanent(longUrl);
		}
	}
}
