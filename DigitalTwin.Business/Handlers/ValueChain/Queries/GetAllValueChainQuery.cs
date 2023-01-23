using DigitalTwin.Business.Services.ValueChainType;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;

namespace DigitalTwin.Business.Handlers.ValueChain.Queries
{
    public class GetAllValueChainQuery : IRequestHandler<GetAllValueChainTypeRequest, Response<List<ValueChainTypeResponse>>>
    {
        private readonly IValueChainTypeService _service;
        public GetAllValueChainQuery(IValueChainTypeService service)
        {
            _service = service;
        }

        public async Task<Response<List<ValueChainTypeResponse>>> Handle(GetAllValueChainTypeRequest request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(cancellationToken);
        }
    }
}
