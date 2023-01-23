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
    public class GetValueChainDetailQuery : IRequestHandler<ViewBusinessDashboardRequest, Response<ValueChainDetailResponse>>
    {
        private readonly IValueChainService _valueChainService;

        public GetValueChainDetailQuery(IValueChainService valueChainService)
        {
            _valueChainService = valueChainService;
        }

        public async Task<Response<ValueChainDetailResponse>> Handle(ViewBusinessDashboardRequest request, CancellationToken cancellationToken)
        {
            return await _valueChainService.GetValueChainDetail(request, cancellationToken);
        }
    }
}
