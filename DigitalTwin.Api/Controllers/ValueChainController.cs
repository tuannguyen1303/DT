using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers
{
    public class ValueChainController : BaseController
    {
        public ValueChainController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<Response<List<ValueChainTypeResponse>>> GetAll([FromBody] GetAllValueChainTypeRequest request, CancellationToken token)
        {
            var response = await _mediator.Send(request, token);
            return response;
        }

        [HttpPost]
        public async Task<Response<ValueChainResponse>> GetAllValueChain([FromBody] ValueChainRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            return result;
        }


        [HttpPost]
        public async Task<Response<GetAllFrequencyResponse>> GetAllFrequency(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllFrequencyRequest(), token);
            return result;
        }

        [HttpPost]
        public async Task<Response<ValueChainDetailResponse>> GetValueChainDetail([FromBody] ViewBusinessDashboardRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            return result;
        }

        [HttpPost]
        public async Task<Response<GetAllKPIResponse>> GetAllKPI([FromBody] GetAllKPIRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            return result;
        }

        [HttpPost]
        public async Task<Response<GetProductDetailResponse>> GetProductDetail([FromBody] GetProductDetailRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            return result;
        }
    }
}
