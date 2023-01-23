using DigitalTwin.Business.Handlers.Entity.Queries;
using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.Entity;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Handlers.Entity.Query
{
    public class GetAllEntitiesQueryTest
    {
        private readonly Mock<IEntityService> _mockEntityService;

        public GetAllEntitiesQueryTest()
        {
            _mockEntityService = new Mock<IEntityService>();
        }
        [Fact]
        public async Task GetEntities_Success_HasData()
        {
            var request = new GetAllEntitiesRequest
            {
                ValueChainType = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                Filters = new List<string>
                 {
                     "plant", "opu", "product"
                 },
                FromDate = DateTime.Parse("2021-12-01T08:50:31.725Z"),
                ToDate = DateTime.Parse("2021-12-31T08:50:31.725Z")
            };

            var query = new GetAllEntitiesQuery(_mockEntityService.Object);

            var entityData = new GetEntitiesFilterAdvancedResponse
            {
                EntitiesByTypeResponses = new List<EntitiesByTypeResponse>
                 {
                     new EntitiesByTypeResponse
                     {
                     EntityGroupName = "Plant",
                     Count = 1,
                     Entities = new List<EntityResponse>
                         {
                             new EntityResponse
                             {
                                 Id = Guid.NewGuid(),
                                 EntityId =  Guid.Parse("c7ac3c65-0c0b-4ae7-bfb9-4f1169f647f6"),
                                 Name = "PCE",
                                 Alias = "pce",
                                 Type = "PCE",
                                 Path = "Downstream > PCGB > PCEPE > PCE",
                                 Status = null,
                                 Category = "Plant"
                             },
                         }
                     }
                 }
            };

            var expected = Response.CreateResponse(entityData);
            _mockEntityService.Setup(p => p.GetAllEntities(It.IsAny<GetAllEntitiesRequest>(), default)).ReturnsAsync(expected);
            var result = await query.Handle(request, default);
            Assert.True(result?.Result?.EntitiesByTypeResponses?.Count > 0);
        }
    }
}
