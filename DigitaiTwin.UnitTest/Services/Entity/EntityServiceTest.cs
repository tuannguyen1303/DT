using DigitalTwin.Business.Services.Chart;
using DigitalTwin.Business.Services.Entity;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Data.Entities;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Services.Entity
{
    public class EntityServiceTest
    {
        private Mock<IValueChainService> _valueChainServiceMock;
        private IEntityService _mockEntityService;
        private Mock<IChartService> _chartServiceMock;
        private ConnectionFactory _connectionFactory = new ConnectionFactory();

        public EntityServiceTest()
        {
            _valueChainServiceMock = new Mock<IValueChainService>();
            _chartServiceMock = new Mock<IChartService>();
            _mockEntityService =
                new EntityService(_valueChainServiceMock.Object);
        }

        [Fact]
        public async Task GetAllEntities_Success()
        {
            var context = _connectionFactory.CreateContextForInMemory();

            var entities = new List<DigitalTwin.Data.Entities.Entity>
             {
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                     Id = Guid.Parse("5a4c9e6d-62ff-77b6-7405-35ad9e9413b7"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                     Id = Guid.Parse("abaea8e2-c80b-2883-c918-16071c7d7773"),
                     EntityTypeId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06265"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 436,
                     Depth = 3,
                     Name = "BA",
                     KpiPath = "Downstream > PCGB > PCD > BA"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                     Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813b"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                     Id = Guid.Parse("2447cd46-9a22-2e07-36ef-058d6032e601"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityTypeMasterId = 401,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
             };
            context.Entities?.AddRange(entities);

            var entityTypes = new List<EntityType>
             {
                 new EntityType
                 {
                     Id = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     Name = "MTBE",
                     EntityGroupName = "Plant",
                     EntityId = 405,
                     FullName = "Methyl Tertiary Butyl Ether",
                 },
                 new EntityType
                 {
                     Id = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06265"),
                     Name = "BA",
                     EntityGroupName = "Plant",
                     EntityId = 436,
                     FullName = "Butyl Acetate",
                 },
             };
            context.EntityTypes?.AddRange(entityTypes);

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
            context.ValueChainTypes?.AddRange(valueChainTypeData);

            var products = new List<ProductLink>
             {
                 new ProductLink
                 {
                     Id = Guid.Parse("e943adec-df2d-4d3a-a28d-66b543e98ed9"),
                     UnitOfMeasureId = Guid.Parse("2323c6ad-3f41-4242-a536-be821f817c9e"),
                     EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     FullName = "Production Volume",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("6958f9c0-3078-4962-9038-7efbc0705768"),
                     UnitOfMeasureId = Guid.Parse("2323c6ad-3f41-4242-a536-be821f817c9e"),
                     EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     FullName = "Plant Utilization",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("7ee3ef31-5921-4c0f-b9c0-2e860988bd03"),
                     UnitOfMeasureId = Guid.Parse("2323c6ad-3f41-4242-a536-be821f817c9e"),
                     EntityMapId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Name = "Plant Utilization",
                     FullName = "Plant Utilization",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49921"),
                     UnitOfMeasureId = Guid.Parse("2323c6ad-3f41-4242-a536-be821f817c9e"),
                     EntityMapId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Name = "Plant Utilization",
                     FullName = "Plant Utilization",
                 },
             };
            context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
             {
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("6958f9c0-3078-4962-9038-7efbc0705768"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color = "red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                     NumValues =
                         "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("e943adec-df2d-4d3a-a28d-66b543e98ed9"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color = "red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues =
                         "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("7ee3ef31-5921-4c0f-b9c0-2e860988bd03"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color = "red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues =
                         "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color = "red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues =
                         "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
             };
            context.ProductLinkDetails?.AddRange(productLinkDetails);

            var uom = new List<UnitOfMeasure>
             {
                 new UnitOfMeasure
                 {
                     Name = "mmscfd",
                     Id = Guid.Parse("2323c6ad-3f41-4242-a536-be821f817c9e"),
                 },
                 new UnitOfMeasure
                 {
                     Name = "kbpd",
                     Id = Guid.Parse("23119d1d-d653-455a-a446-0c2e09e50dda"),
                 },
             };
            context.UnitOfMeasures?.AddRange(uom);
            context.SaveChanges();

            var request = new GetAllEntitiesRequest
            {
                ValueChainType = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                Filters = new List<string>
                 {
                     "plant", "opu", "product"
                 },
                FromDate = DateTime.Parse("2021-06-01T08:50:31.725Z"),
                ToDate = DateTime.Parse("2021-06-30T08:50:31.725Z"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };

            var listEntities = new List<EntityDto>
             {
                 new EntityDto
                 {
                     Id = Guid.Parse("5a4c9e6d-62ff-77b6-7405-35ad9e9413b7"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "mtbe"
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("abaea8e2-c80b-2883-c918-16071c7d7773"),
                     EntityTypeId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06265"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 436,
                     Name = "BA",
                     Type = "BA",
                     Category = "Plant",
                     Alias = "ba"
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813b"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "ba"
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("2447cd46-9a22-2e07-36ef-058d6032e601"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityTypeMasterId = 401,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "ba"
                 },
             };

            var listProducts = new List<ProductDto>
            {
            };

            _valueChainServiceMock.Setup(p => p.GetValueChains(It.IsAny<ValueChainRequest>(), default))
                .ReturnsAsync(new ValueChainResponse
                {
                    Title = "Gas Chain",
                    Entities = listEntities,
                    Products = listProducts
                });

            var result = await _mockEntityService.GetAllEntities(request, default);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllEntitiesNo_Success()
        {
            var context = _connectionFactory.CreateContextForInMemory();

            var entities = new List<DigitalTwin.Data.Entities.Entity>
             {
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("b7b4e1b5-7b2a-4f32-b783-6c4785736683"),
                     Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813f"),
                     EntityTypeId = Guid.Parse("f44abfeb-f735-47b5-86dd-fc71b6a70edb"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("b7b4e1b5-7b2a-4f32-b783-6c4785736683"),
                     Id = Guid.Parse("c7f47571-f910-95d2-f54f-fa3c082eac1c"),
                     EntityTypeId = Guid.Parse("4de5c469-90e9-4d19-ab12-674bb69efe38"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityTypeMasterId = 436,
                     Depth = 3,
                     Name = "BA",
                     KpiPath = "Downstream > PCGB > PCD > BA"
                 },
             };
            context.Entities?.AddRange(entities);

            var entityTypes = new List<EntityType>
             {
                 new EntityType
                 {
                     Id = Guid.Parse("f44abfeb-f735-47b5-86dd-fc71b6a70edb"),
                     Name = "MTBE",
                     EntityGroupName = "Plant",
                     EntityId = 405,
                     FullName = "Methyl Tertiary Butyl Ether",
                 },
                 new EntityType
                 {
                     Id = Guid.Parse("4de5c469-90e9-4d19-ab12-674bb69efe38"),
                     Name = "BA",
                     EntityGroupName = "Plant",
                     EntityId = 436,
                     FullName = "Butyl Acetate",
                 },
             };
            context.EntityTypes?.AddRange(entityTypes);

            var valueChainTypeData = new List<DigitalTwin.Data.Entities.ValueChainType>
             {
                 new DigitalTwin.Data.Entities.ValueChainType
                 {
                     Id = Guid.Parse("b7b4e1b5-7b2a-4f32-b783-6c4785736683"),
                     Name = "Gas chain",
                     Type = ChainType.Gas
                 },
                 new DigitalTwin.Data.Entities.ValueChainType
                 {
                     Id = Guid.Parse("9fc0ce68-c702-45f6-855d-2b3e90c505df"),
                     Name = "Oil chain",
                     Type = ChainType.Oil
                 }
             };
            context.ValueChainTypes?.AddRange(valueChainTypeData);

            var products = new List<ProductLink>
             {
                 new ProductLink
                 {
                     Id = Guid.Parse("3ea97a57-e865-4c4d-a2a8-e12e8da1e96b"),
                     UnitOfMeasureId = Guid.Parse("19c58def-fd73-446f-a52b-7d1a17628c61"),
                     EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     FullName = "Production Volume",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                     UnitOfMeasureId = Guid.Parse("19c58def-fd73-446f-a52b-7d1a17628c61"),
                     EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     FullName = "Plant Utilization",
                 },
             };
            context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
             {
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color = "red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                     NumValues =
                         "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color = "red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues =
                         "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 }
             };
            context.ProductLinkDetails?.AddRange(productLinkDetails);

            var uom = new List<UnitOfMeasure>
             {
                 new UnitOfMeasure
                 {
                     Name = "mmscfd",
                     Id = Guid.Parse("19c58def-fd73-446f-a52b-7d1a17628c61"),
                 },
                 new UnitOfMeasure
                 {
                     Name = "kbpd",
                     Id = Guid.Parse("9fcb357f-8488-499e-a278-b543749797af"),
                 },
             };
            context.UnitOfMeasures?.AddRange(uom);
            context.SaveChanges();

            var request = new GetAllEntitiesRequest
            {
                ValueChainType = Guid.Parse("b7b4e1b5-7b2a-4f32-b783-6c4785736683"),
                Filters = new List<string>
                 {
                     "plant", "opu", "product"
                 },
                FromDate = DateTime.Parse("2021-06-01T08:50:31.725Z"),
                ToDate = DateTime.Parse("2021-06-30T08:50:31.725Z"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var listEntities = new List<EntityDto>
             {
                 new EntityDto
                 {
                     Id = Guid.Parse("5a4c9e6d-62ff-77b6-7405-35ad9e9413b7"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "mtbe"
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("abaea8e2-c80b-2883-c918-16071c7d7773"),
                     EntityTypeId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06265"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 436,
                     Name = "BA",
                     Type = "BA",
                     Category = "Plant",
                     Alias = "ba"
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813b"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "ba"
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("2447cd46-9a22-2e07-36ef-058d6032e601"),
                     EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537018"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityTypeMasterId = 401,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "ba"
                 },
             };

            var listProducts = new List<ProductDto>
            {
            };

            _valueChainServiceMock.Setup(p => p.GetValueChains(It.IsAny<ValueChainRequest>(), default))
                .ReturnsAsync(new ValueChainResponse
                {
                    Title = "Gas Chain",
                    Entities = listEntities,
                    Products = listProducts
                });
            var result = await _mockEntityService.GetAllEntities(request, default);
            Assert.NotNull(result);
        }
    }
}