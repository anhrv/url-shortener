using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Domain.Models;

namespace UrlShortener.Infrastructure
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext()
		{
		}
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<UrlObject> UrlObject { get; set; }
	}
}
