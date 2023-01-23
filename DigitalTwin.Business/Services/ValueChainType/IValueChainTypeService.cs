using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;

namespace DigitalTwin.Business.Services.ValueChainType
{
    public interface IValueChainTypeService
    {
        Task<Response<List<ValueChainTypeResponse>>> GetAll(CancellationToken token);
    }
}
