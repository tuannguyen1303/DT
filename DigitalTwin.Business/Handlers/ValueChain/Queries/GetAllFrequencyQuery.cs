using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;

namespace DigitalTwin.Business.Handlers.ValueChain.Queries;

public class GetAllFrequencyQuery : IRequestHandler<GetAllFrequencyRequest, Response<GetAllFrequencyResponse>>
{
    private readonly IValueChainService _valueChainService;

    public GetAllFrequencyQuery(IValueChainService valueChainService)
    {
        _valueChainService = valueChainService;
    }


    public async Task<Response<GetAllFrequencyResponse>> Handle(GetAllFrequencyRequest request, CancellationToken cancellationToken)
    {
        return await _valueChainService.GetFrequencies(cancellationToken);
    }
}