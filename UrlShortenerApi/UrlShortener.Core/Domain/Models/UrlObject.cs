using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Core.Domain.Models
{
	[Table("UrlObject")]
	public class UrlObject
	{
        [Key]
        public Guid UrlId { get; set; }

        [MaxLength(2048, ErrorMessage = "The maximum length of a URL is 2048 characters.")]
        public string LongUrl { get; set; }

        [MaxLength(6, ErrorMessage = "The maximum length of a key is 6 characters.")]
        public string Key { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The minimum number of clicks is 1.")]
        public int? RemainingClicks { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
