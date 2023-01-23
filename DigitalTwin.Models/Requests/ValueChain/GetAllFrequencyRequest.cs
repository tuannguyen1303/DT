using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;

namespace DigitalTwin.Models.Requests.ValueChain;

public class GetAllFrequencyRequest : BaseRequest, IRequest<Response<GetAllFrequencyResponse>>
{
}