using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Domain.Models;
using UrlShortener.Core.Domain.RepositoryContracts;

namespace UrlShortener.Infrastructure.Repositories
{
	public class UrlRepository : IUrlRepository
	{
		private readonly ApplicationDbContext _db;

		public UrlRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<UrlObject> AddUrlObject(UrlObject urlObject)
		{
			await _db.UrlObject.AddAsync(urlObject);
			await _db.SaveChangesAsync();

			return urlObject;
		}

		public async Task<UrlObject?> GetUrlObjectByLong(string longUrl)
		{
			var urlObject = await _db.UrlObject.FirstOrDefaultAsync(x => x.LongUrl == longUrl && x.ExpirationDate == null && x.RemainingClicks == null);
			return urlObject;
		}

		public async Task<UrlObject?> GetUrlObjectByShort(string key)
		{
			var urlObject = await _db.UrlObject.FirstOrDefaultAsync(x => x.Key == key);
			return urlObject;
		}

		public async Task DeleteUrlObject(UrlObject urlObject)
		{
			_db.UrlObject.Remove(urlObject);
			await _db.SaveChangesAsync();
		}

		public async Task UseOneClick(UrlObject urlObject)
		{
			urlObject.RemainingClicks--;
			if(urlObject.RemainingClicks == 0)
				_db.UrlObject.Remove(urlObject);
			await _db.SaveChangesAsync();
		}
	}
}
