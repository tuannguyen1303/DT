using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Business.Services.ValueChainType;
using DigitalTwin.Data.Database;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Data.Entities;

namespace DigitaiTwin.UnitTest.Services.ValueChainType
{
    public class ValueChainTypeServiceTest
    {
        private IValueChainTypeService _mockValueChainTypeService;
        private ConnectionFactory _factory = new ConnectionFactory();
        public ValueChainTypeServiceTest()
        {
            _mockValueChainTypeService = new ValueChainTypeService(_factory.CreateContextForInMemory());
        }

        [Fact]
        public async Task GetValueChainType_Success()
        {
            var request = new GetAllValueChainTypeRequest();

            var valueChainTypeData = new List<DigitalTwin.Data.Entities.ValueChainType>
            {
               new DigitalTwin.Data.Entities.ValueChainType
               {
                   Id = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                   Name = "Gas chain",
                   Type = ChainType.Gas
               },
               new DigitalTwin.Data.Entities.ValueChainType
               {
                   Id = Guid.Parse("faf7a077-4214-acc6-22d0-82597e755126"),
                   Name = "Oil chain",
                   Type = ChainType.Oil
               }
            };

            //Get the instance of BlogDBContext  
            var context = _factory.CreateContextForInMemory();

            //Act    
            context.ValueChainTypes?.AddRange(valueChainTypeData);
            context.SaveChanges();

            var result = await _mockValueChainTypeService.GetAll(new CancellationToken());
            Assert.NotNull(result);
        }
    }
}
