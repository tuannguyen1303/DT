using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Business.Services.ValueChain
{
    public interface IValueChainService
    {
        Task<Response<ValueChainResponse>> GetValueChainByType(ValueChainRequest request, CancellationToken token);
        Task<ValueChainResponse> GetValueChains(ValueChainRequest request, CancellationToken token);
        Task<Response<GetAllFrequencyResponse>> GetFrequencies(CancellationToken token);

        Task<Response<ValueChainDetailResponse>> GetValueChainDetail(ViewBusinessDashboardRequest request,
            CancellationToken token);

        Task<Response<GetAllKPIResponse>> GetAllKPI(GetAllKPIRequest request, CancellationToken token);

        Task<Response<GetProductDetailResponse>> GetProductDetail(GetProductDetailRequest request,
            CancellationToken token);
    }
}