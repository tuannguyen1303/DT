using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;

namespace DigitalTwin.Models.Requests.ValueChain
{
    public class GetAllValueChainTypeRequest : BaseRequest, IRequest<Response<List<ValueChainTypeResponse>>>
    {
    }
}
