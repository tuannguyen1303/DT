using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.Chart;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Handlers.ValueChain.Queries
{
    public class GetProductDetailQueryTest
    {
        private readonly Mock<IValueChainService> _mockValueChainService;
        private readonly GetProductDetailQuery _handler;

        public GetProductDetailQueryTest()
        {
            _mockValueChainService = new Mock<IValueChainService>();
            _handler = new GetProductDetailQuery(_mockValueChainService.Object);
        }

        [Fact]
        public async Task GetProductDetailQuery_Success()
        {
            var response = new GetProductDetailResponse
            {
                ChartData = new ChartData(),
                IsShowChart = true,
                Data = new DataValueChainDetail(),
                ProductionVolume = new List<ProductionVolume>(),
                IsNonFrequency = true,
                IsTarget = true,
            };

            var expected = Response.CreateResponse(response);
            _mockValueChainService.Setup(p => p.GetProductDetail(It.IsAny<GetProductDetailRequest>(), default)).ReturnsAsync(expected);

            var result = await _handler.Handle(new GetProductDetailRequest(), default);
            Assert.NotNull(result.Result);
            Assert.Equal(result?.Result?.ProductionVolume?.Count, expected?.Result?.ProductionVolume?.Count);
        }
    }
}
