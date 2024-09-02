using System.Diagnostics;

namespace CommonLibrary.Extensions
{
    public class ApiException : Exception
    {
        public string className { get; set; }

        public ApiException(string message) : base(message)
        {
            className = GetCallingClassName();
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
            className = GetCallingClassName();
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
