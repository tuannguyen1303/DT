using DigitalTwin.Models.Responses.Exceptions;

namespace DigitalTwin.Models.Responses
{
    public abstract class BaseResponse
    {
        public int StatusCode { get; set; } = 200;

        public string StatusText { get; set; } = string.Empty;

        public bool IsError => Errors != null && Errors.Any();

        public IEnumerable<BaseResponseException>? Errors { get; set; }
    }
}
