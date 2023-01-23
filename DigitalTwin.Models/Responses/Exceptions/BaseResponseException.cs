namespace DigitalTwin.Models.Responses.Exceptions
{
    public class BaseResponseException
    {
        public string? ErrorCode { get; set; }

        public string? Message { get; set; }

        public string? FullMessage { get; set; }

        public object? ErrorData { get; set; }
    }
}
