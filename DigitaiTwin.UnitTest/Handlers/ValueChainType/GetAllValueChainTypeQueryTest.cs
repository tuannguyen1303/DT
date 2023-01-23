using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Business.Services.ValueChainType;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Handlers.ValueChainType
{
    public class GetAllValueChainTypeQueryTest
    {
        private readonly Mock<IValueChainTypeService> _mockValueChainTypeService;
        public GetAllValueChainTypeQueryTest()
        {
            _mockValueChainTypeService = new Mock<IValueChainTypeService>();
        }

        [Fact]
        public async Task GetProduct_Success_HasData()
        {
            var request = new GetAllValueChainTypeRequest();

            var query = new GetAllValueChainQuery(_mockValueChainTypeService.Object);

            var valueChainTypeData = new List<ValueChainTypeResponse>
            {
               new ValueChainTypeResponse
               {
                   Id = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                   Name = "Gas chain",
                   Label = "Gas"
               },
               new ValueChainTypeResponse
               {
                   Id = Guid.Parse("faf7a077-4214-acc6-22d0-82597e755126"),
                   Name = "Oil chain",
                   Label = "Oil"
               }
            };

            var expected = Response.CreateResponse(valueChainTypeData);

            _mockValueChainTypeService.Setup(p => p.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(expected);
            var result = await query.Handle(request, default);

            Assert.True(result?.Result?.Count == 2);
        }
    }
}
