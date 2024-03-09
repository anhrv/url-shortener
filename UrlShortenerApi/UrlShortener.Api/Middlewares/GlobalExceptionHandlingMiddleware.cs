using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using UrlShortener.Core.DTO;

namespace UrlShortener.Api.Middlewares
{
	public class GlobalExceptionHandlingMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch(Exception ex)
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var apiError = new ErrorResponse();

				context.Response.ContentType= "application/json";

				string jsonError = JsonConvert.SerializeObject(apiError,new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver()});

				await context.Response.WriteAsync(jsonError);
			}
		}
	}
}
