using DigitalTwin.Models.Requests.Chart;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Chart;

namespace DigitalTwin.Business.Services.Chart
{
    public interface IChartService
    {
        ChartData GetChart(ChartRequest request, CancellationToken token);
        Task<Response<ChartResponse>> GetChartData(ChartRequest request, CancellationToken token);
    }
}
