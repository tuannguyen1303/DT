using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Business.Handlers.ValueChain.Queries
{
    public class GetProductDetailQuery : IRequestHandler<GetProductDetailRequest, Response<GetProductDetailResponse>>
    {
        private readonly IValueChainService _valueChainService;

        public GetProductDetailQuery(IValueChainService valueChainService)
        {
            _valueChainService = valueChainService;
        }

        public async Task<Response<GetProductDetailResponse>> Handle(GetProductDetailRequest request, CancellationToken cancellationToken)
        {
            return await _valueChainService.GetProductDetail(request, cancellationToken);
        }
    }
}
