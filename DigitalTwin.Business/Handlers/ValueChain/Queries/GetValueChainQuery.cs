using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;

namespace DigitalTwin.Business.Handlers.ValueChain.Queries
{
    public class GetValueChainQuery : IRequestHandler<ValueChainRequest, Response<ValueChainResponse>>
    {
        private readonly IValueChainService _valueChainService;

        public GetValueChainQuery(IValueChainService valueChainService)
        {
            _valueChainService = valueChainService;
        }

        public async Task<Response<ValueChainResponse>> Handle(ValueChainRequest request,
            CancellationToken cancellationToken)
        {
            return await _valueChainService.GetValueChainByType(request, cancellationToken);
        }
    }
}