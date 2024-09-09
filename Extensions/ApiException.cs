using System.Diagnostics;
using System.Net;

namespace CommonLibrary.Extensions
{
    public class ApiException : Exception
    {
        public string ClassName { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError; // 預設為500 Internal Server Error

        public ApiException(string message) : base(message)
        {
            ClassName = GetCallingClassName();
        }

        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            ClassName = GetCallingClassName();
            StatusCode = statusCode; // 自訂的狀態碼
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
            ClassName = GetCallingClassName();
        }

        public ApiException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            ClassName = GetCallingClassName();
            StatusCode = statusCode; // 自訂的狀態碼
        }

        private string GetCallingClassName()
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();

            var frame = frames.FirstOrDefault(f =>
                f.GetMethod()!.DeclaringType != null &&
                !f.GetMethod()!.DeclaringType!.FullName!.StartsWith("System.") &&
                !f.GetMethod()!.DeclaringType!.FullName!.StartsWith("Microsoft.") &&
                !f.GetMethod()!.DeclaringType!.FullName!.StartsWith("ExceptionHandling") &&
                f.GetMethod()!.DeclaringType != typeof(ApiException)
            );

            if (frame != null)
            {
                var method = frame.GetMethod();
                var className = method!.DeclaringType?.FullName;
                var methodName = method.Name;
                return $"{className}.{methodName}";
            }

            return "UnknownMethod";
        }
    }
}
