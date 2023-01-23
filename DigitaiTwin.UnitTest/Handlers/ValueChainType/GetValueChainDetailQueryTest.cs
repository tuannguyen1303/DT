using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.Chart;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Business.Services.ValueChainType;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Handlers.ValueChainType
{
    public class GetValueChainDetailQueryTest
    {
        private readonly Mock<IValueChainService> _mockValueChainService;
        private readonly Mock<IChartService> _mockChartService;
        public GetValueChainDetailQueryTest()
        {
            _mockChartService = new Mock<IChartService>();
            _mockValueChainService = new Mock<IValueChainService>();
        }

        [Fact]
        public async Task GetProduct_Success_HasData()
        {
            var request = new ViewBusinessDashboardRequest();

            var query = new GetValueChainDetailQuery(_mockValueChainService.Object);

            var valueChainDetailData = "{\r\n    \"result\": {\r\n        \"chartData\": {\r\n            \"actual\": [\r\n                \"4.21\",\r\n                \"3.14\",\r\n                \"3.42\",\r\n                \"4.44\",\r\n                \"3.23\",\r\n                \"4.58\",\r\n                \"4.48\",\r\n                \"4.02\",\r\n                \"3.53\",\r\n                \"4.09\",\r\n                \"4.50\",\r\n                \"3.28\"\r\n            ],\r\n            \"target\": [\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\"\r\n            ],\r\n            \"planned\": [\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\",\r\n                \"3.75\"\r\n            ],\r\n            \"labels\": [\r\n                \"Jan 2021\",\r\n                \"Feb 2021\",\r\n                \"Mar 2021\",\r\n                \"Apr 2021\",\r\n                \"May 2021\",\r\n                \"Jun 2021\",\r\n                \"Jul 2021\",\r\n                \"Aug 2021\",\r\n                \"Sep 2021\",\r\n                \"Oct 2021\",\r\n                \"Nov 2021\",\r\n                \"Dec 2021\"\r\n            ],\r\n            \"forecastBarChart\": [\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null\r\n            ],\r\n            \"forecastActual\": [\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null\r\n            ],\r\n            \"currentForecast\": [\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null,\r\n                null\r\n            ],\r\n            \"currentIndex\": 7,\r\n            \"currentIndexAnalysis\": 7,\r\n            \"kpbi\": [\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\",\r\n                \"0\"\r\n            ],\r\n            \"isShowChart\": true,\r\n            \"title\": \"kMT\"\r\n        },\r\n        \"performanceAnalysis\": null,\r\n        \"productionVolume\": [\r\n            {\r\n                \"name\": \"Isobutylene\",\r\n                \"actual\": \"8.50\",\r\n                \"planned\": \"7.50\",\r\n                \"variance\": \"1.00\",\r\n                \"kpbi\": null,\r\n                \"id\": \"71e897a2-c6ca-8241-740c-0c4a9fb201ba\",\r\n                \"unit\": \"kMT\",\r\n                \"alias\": null,\r\n                \"color\": null\r\n            },\r\n            {\r\n                \"name\": \"MTBE\",\r\n                \"actual\": \"48.13\",\r\n                \"planned\": \"50.18\",\r\n                \"variance\": \"-2.05\",\r\n                \"kpbi\": null,\r\n                \"id\": \"6041196f-bd8a-409d-b0a0-2d388b0b5594\",\r\n                \"unit\": \"kMT\",\r\n                \"alias\": null,\r\n                \"color\": null\r\n            },\r\n            {\r\n                \"name\": \"Propylene\",\r\n                \"actual\": \"12.93\",\r\n                \"planned\": \"10.16\",\r\n                \"variance\": \"2.77\",\r\n                \"kpbi\": null,\r\n                \"id\": \"20504958-430b-f17b-613c-8423cd0ef265\",\r\n                \"unit\": \"kMT\",\r\n                \"alias\": null,\r\n                \"color\": null\r\n            }\r\n        ],\r\n        \"feedstockSupply\": null,\r\n        \"salesVolume\": null,\r\n        \"data\": {\r\n            \"actual\": \"69.56\",\r\n            \"id\": \"ce92cb67-9364-49d5-a590-53d69b72c73c\",\r\n            \"name\": \"Downstream > PCGB > PCMTBE > Production Volume\",\r\n            \"target\": \"67.84\",\r\n            \"unit\": \"kMT\",\r\n            \"variance\": \"1.72\",\r\n            \"targetLabel\": \"Planned\",\r\n            \"isEntityExist\": true,\r\n            \"kpbi\": \"67.84\",\r\n            \"status\": \"Running\",\r\n            \"justification\": \"\",\r\n            \"createdDate\": \"2022-11-16T06:21:23.080663Z\",\r\n            \"index\": 0,\r\n            \"variancePercentage\": \"1.72\",\r\n            \"titleChart\": \"Production Volume (Monthly)\",\r\n            \"kpi_Id\": \"eabe8ffe-329f-4bcc-a5bd-fe051b517a8b\"\r\n        },\r\n        \"isShowChart\": true,\r\n        \"isNonFrequency\": false,\r\n        \"isTarget\": false\r\n    },\r\n    \"statusCode\": 200,\r\n    \"statusText\": \"\",\r\n    \"isError\": false,\r\n    \"errors\": null\r\n}";

            var dataReal = JsonConvert.DeserializeObject<ValueChainDetailResponse>(valueChainDetailData);

            _mockValueChainService.Setup(p => p.GetValueChainDetail(It.IsAny<ViewBusinessDashboardRequest>(), default)).Returns(Task.FromResult(Response.CreateResponse(dataReal)));
            var result = await query.Handle(request, default);

            Assert.NotNull(result);
        }
    }
}
