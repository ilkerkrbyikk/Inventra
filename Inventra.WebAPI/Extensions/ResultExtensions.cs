using Inventra.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Inventra.WebAPI.Extensions
{
    /// <summary>
    /// Extension methods for mapping Result objects to HTTP responses.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Maps a Result to an IActionResult.
        /// Success returns 200 OK with the message.
        /// Failure returns 400 BadRequest with errors.
        /// </summary>
        public static IActionResult ToHttpResponse(this Result result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(new
                {
                    success = true,
                    message = result.Message
                });
            }

            return new BadRequestObjectResult(new
            {
                success = false,
                errors = result.Errors
            });
        }

        /// <summary>
        /// Maps a Result to an IActionResult with custom HTTP status code for success.
        /// </summary>
        public static IActionResult ToHttpResponse(this Result result, int successStatusCode)
        {
            if (result.IsSuccess)
            {
                return new ObjectResult(new
                {
                    success = true,
                    message = result.Message
                }) { StatusCode = successStatusCode };
            }

            return new BadRequestObjectResult(new
            {
                success = false,
                errors = result.Errors
            });
        }

        /// <summary>
        /// Maps a Result<TData> to an IActionResult.
        /// Success returns 200 OK with the data.
        /// Failure returns 404 NotFound with errors if data is null expectation, else 400 BadRequest.
        /// </summary>
        public static IActionResult ToHttpResponse<TData>(this Result<TData> result)
        {
            if (result.IsSuccess && result.Data is not null)
            {
                return new OkObjectResult(new
                {
                    success = true,
                    message = result.Message,
                    data = result.Data
                });
            }

            // If failure suggests "not found", return 404; otherwise 400
            var isNotFound = result.Errors.Any(e =>
                e.Contains("not found", StringComparison.OrdinalIgnoreCase) ||
                e.Contains("does not exist", StringComparison.OrdinalIgnoreCase));

            if (isNotFound)
            {
                return new NotFoundObjectResult(new
                {
                    success = false,
                    errors = result.Errors
                });
            }

            return new BadRequestObjectResult(new
            {
                success = false,
                errors = result.Errors
            });
        }

        /// <summary>
        /// Maps a Result<TData> to an IActionResult with custom HTTP status code for success.
        /// </summary>
        public static IActionResult ToHttpResponse<TData>(this Result<TData> result, int successStatusCode)
        {
            if (result.IsSuccess && result.Data is not null)
            {
                return new ObjectResult(new
                {
                    success = true,
                    message = result.Message,
                    data = result.Data
                }) { StatusCode = successStatusCode };
            }

            var isNotFound = result.Errors.Any(e =>
                e.Contains("not found", StringComparison.OrdinalIgnoreCase) ||
                e.Contains("does not exist", StringComparison.OrdinalIgnoreCase));

            if (isNotFound)
            {
                return new NotFoundObjectResult(new
                {
                    success = false,
                    errors = result.Errors
                });
            }

            return new BadRequestObjectResult(new
            {
                success = false,
                errors = result.Errors
            });
        }
    }
}