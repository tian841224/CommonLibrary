using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace admin_backend.Middleware
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
            string FILE_PATH = "/getfile";
            var notConvertUrlList = new List<string>() { STATICS_PATH, SWAGGER_PATH, FILE_PATH };

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

            string className = string.Empty;

            if (exception is ApiException apiException)
            {
                className = apiException.className;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogInformation($"{className}-{exception.Message}");
                var result = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(result);
            }
            else
            {
                switch (exception)
                {
                    case ArgumentNullException _:
                    case ArgumentException _:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UnauthorizedAccessException _:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case NotImplementedException _:
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    case ApiException _:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }


                _logger.LogError($"{className}-{exception.Message}");

                var result = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
