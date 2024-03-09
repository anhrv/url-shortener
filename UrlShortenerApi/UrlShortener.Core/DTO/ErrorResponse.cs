using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Core.DTO
{
	public class ErrorResponse
	{
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public List<string> Errors { get; } = new List<string>();
        public DateTime Timestamp { get; set; }

        public ErrorResponse(int statusCode = 500, string status = "Internal Server Error", string errorMessage="Something went wrong!")
        {
            StatusCode = statusCode;
            Status = status;
            Errors.Add(errorMessage);
            Timestamp = DateTime.Now;
        }

        public static IActionResult GenerateValidationErrorResponse(ActionContext context)
        {
            var apiError = new ErrorResponse();
            apiError.StatusCode = 400;
            apiError.Status = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            var errors = context.ModelState.AsEnumerable();

            foreach (var error in errors)
            {
                foreach(var inner in error.Value!.Errors)
                {
                    apiError.Errors.Add(inner.ErrorMessage);
                }
            }

            return new BadRequestObjectResult(apiError);
        }
    }
}
