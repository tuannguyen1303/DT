namespace DigitalTwin.Models.Responses
{

    public class Response<TResponse> : BaseResponse
    {
        public static Response<TResponse> CreateResponse(TResponse result)
        {
            return new Response<TResponse>
            {
                Result = result,
            };
        }

        private Response() { }

        public TResponse? Result { get; set; }

    }

    public class Response : BaseResponse
    {
        public static Response<TResponse> CreateResponse<TResponse>(TResponse result)
        {
            return Response<TResponse>.CreateResponse(result);
        }

        public static Response CreateResponse(object? result)
        {
            return new Response
            {
                Result = result,
            };
        }

        private Response() { }

        public object? Result { get; set; }

    }
}
