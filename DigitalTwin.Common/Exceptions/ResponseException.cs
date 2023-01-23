namespace DigitalTwin.Common.Exceptions
{
    /// <summary>
    /// ResponseException for service runtime
    /// </summary>
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class ResponseException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public static void Throw(int statusCode, string statusText, string message, object data)
        {
            throw new ResponseException(statusCode, statusText, data, message)
            {
                StatusCode = statusCode,
                StatusText = statusText,
                ExceptionAdditionalData = data,
            };
        }

        public ResponseException(int statusCode, string statusText, object data, string message = "") : base(message)
        {
            StatusCode = statusCode;
            StatusText = statusText;
            ExceptionAdditionalData = data;
        }

        public int StatusCode { get; set; }

        public string StatusText { get; set; }

        public object ExceptionAdditionalData { get; set; }
    }
}
