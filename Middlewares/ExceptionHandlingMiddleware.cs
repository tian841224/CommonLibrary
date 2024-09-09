using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CommonLibrary.Middlewares
{
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;
        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string STATICS_PATH = "/statics";
            string SWAGGER_PATH = "/swagger";
            string FILE_PATH = "/file";
            string IMAGE_PATH = "/image";
            var notConvertUrlList = new List<string>() { STATICS_PATH, SWAGGER_PATH, FILE_PATH, IMAGE_PATH };

            if (context.Request.Path.HasValue && notConvertUrlList.Any(p => context.Request.Path.Value.ToLower().Contains(p)))
            {
                await _next(context);
                return;
            }

            var originalResponseBodyStream = context.Response.Body;

            try
            {
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
                    responseBody.Seek(0, SeekOrigin.Begin);

                    if (context.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        var apiResult = new APIResult
                        {
                            Success = true,
                        };

                        if (!string.IsNullOrWhiteSpace(responseBodyText))
                        {
                            var data = JsonSerializer.Deserialize<object>(responseBodyText);
                            apiResult.Data = data;
                        }

                        var jsonResult = JsonSerializer.Serialize(apiResult);
                        await WriteResponseAsync(context, originalResponseBodyStream, jsonResult, "application/json");
                    }
                    else
                    {
                        await responseBody.CopyToAsync(originalResponseBodyStream);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Body = originalResponseBodyStream;
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, Stream responseStream, string content, string contentType)
        {
            context.Response.Body = responseStream;
            context.Response.ContentType = contentType;
            context.Response.ContentLength = Encoding.UTF8.GetByteCount(content);
            await context.Response.WriteAsync(content);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var errorResponse = new APIResult
            {
                Success = false,
                ErrorMsg = exception.Message,
            };

            string className = exception.GetType().Name;
            HttpStatusCode statusCode;

            switch (exception)
            {
                case ApiException apiException:
                    // Handle custom ApiException and assign custom status code
                    className = apiException.ClassName;
                    statusCode = apiException.StatusCode;
                    _logger.LogInformation($"{className}-{exception.Message}");
                    break;

                case ArgumentNullException _:
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case NotImplementedException _:
                    statusCode = HttpStatusCode.NotImplemented;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            response.StatusCode = (int)statusCode;


            _logger.LogError($"{className}-{exception.Message}");

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}

