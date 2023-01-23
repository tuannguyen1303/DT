using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;

namespace DigitaiTwin.UnitTest.Handlers.ValueChain.Queries
{
    public class GetValueChainQueryTest
    {
        private readonly Mock<IValueChainService> _mockValueChainService;
        public GetValueChainQueryTest()
        {
            _mockValueChainService = new Mock<IValueChainService>();
        }

        [Fact]
        public async Task GetProduct_Success_HasData()
        {
            var request = new ValueChainRequest
            {
                ValueChainTypeId = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                FromDate = DateTime.Parse("2021-12-01T08:50:31.725Z"),
                ToDate = DateTime.Parse("2021-12-31T08:50:31.725Z"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                Entities = new List<Guid>()
            };

            var query = new GetValueChainQuery(_mockValueChainService.Object);

            var valueChainData = new ValueChainResponse
            {
                Entities = new List<EntityDto>
                 {
                     new()
                     {
                         Id = Guid.NewGuid(),
                         EntityId =  Guid.Parse("c7ac3c65-0c0b-4ae7-bfb9-4f1169f647f6"),
                         Name = "PCE",
                         Alias = "pce",
                         Type = "PCE",
                         Path = "Downstream > PCGB > PCEPE > PCE",
                         Status = null,
                         Category = "Plant",
                         EntityParentId = Guid.Parse("1ad8c207-c557-47d5-8bb9-44860f9cab3c"),
                         ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>
                         {
                                 new DigitalTwin.Models.Responses.ValueChain.Entity
                                 {
                                     Id = Guid.Parse("830ca6d4-c3e5-ec32-2951-9b9687ff5251"),
                                     Name = "PCEPE",
                                     EntityId = Guid.Parse("1ad8c207-c557-47d5-8bb9-44860f9cab3c"),
                                     Alias = "pcepe",
                                     Type = "PCEPE"
                                 }
                         },
                         ChildrenList = null
                     },
                 },
                Products = new List<ProductDto>
                 {
                     new()
                     {
                         Name = "Routine Maintenance Cost",
                         Alias = "pcepe-pce",
                         Parent = Guid.Parse("1ad8c207-c557-47d5-8bb9-44860f9cab3c"),
                         Children = Guid.Parse("c7ac3c65-0c0b-4ae7-bfb9-4f1169f647f6"),
                         Value = (decimal)11.84,
                         Percentage = (decimal)1376.22,
                         Variance = (decimal)11.04,
                         Unit = "RM Mil",
                         Color = null,
                     }
                 }

            };

            var expected = Response.CreateResponse(valueChainData);

            _mockValueChainService.Setup(p => p.GetValueChainByType(It.IsAny<ValueChainRequest>(), default)).ReturnsAsync(expected);
            var result = await query.Handle(request, default);

            Assert.True(result?.Result?.Entities?.Count > 0);
            Assert.True(result?.Result?.Products?.Count > 0);
        }
    }
}
