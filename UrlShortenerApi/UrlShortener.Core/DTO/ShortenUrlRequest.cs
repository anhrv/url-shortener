using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Core.DTO
{
	public class ShortenUrlRequest
	{
        [Required(ErrorMessage = "You have to enter a URL.")]
        [MaxLength(2048, ErrorMessage = "THe maximum length of a URL is 2048 characters.")]
        public string? LongUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The minimum number of clicks is 1.")]
        public int? RemainingClicks { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The minimum number of minutes is 1.")]
        public int? ExpiresInMinutes { get; set; }
    }
}
