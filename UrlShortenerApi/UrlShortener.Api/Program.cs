using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Middlewares;
using UrlShortener.Core.Domain.RepositoryContracts;
using UrlShortener.Core.DTO;
using UrlShortener.Core.ServiceContracts;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.Build();

			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("sqlServer")));

			builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
			builder.Services.AddScoped<ICacheRepository,CacheRepository>();
			builder.Services.AddScoped<ICacheService, CacheService>();
			builder.Services.AddScoped<IUrlRepository, UrlRepository>();
			builder.Services.AddScoped<IUrlService, UrlService>();

			builder.Services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = config.GetConnectionString("redis");
				options.InstanceName = "urlShortener_";
			});

			builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => {
				options.InvalidModelStateResponseFactory = ErrorResponse.GenerateValidationErrorResponse;
			});

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors(options => options
										.SetIsOriginAllowed(x => _ = true)
										.AllowAnyMethod()
										.AllowAnyHeader()
										.AllowCredentials());	

			app.UseHsts();

			app.UseHttpsRedirection();

			app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

			app.MapControllers();

			app.Run();
		}
	}
}
