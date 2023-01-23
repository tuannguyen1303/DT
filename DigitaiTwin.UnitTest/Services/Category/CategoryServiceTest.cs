using DigitalTwin.Business.Handlers.Entity.Queries;
using DigitalTwin.Business.Services.Category;
using DigitalTwin.Business.Services.Chart;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Data.Database;
using DigitalTwin.Data.Entities;
using DigitalTwin.Models.Requests.Category;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;
using DigitalTwin.Models.Responses.ValueChain;
using Microsoft.AspNetCore.Connections;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Services.Category
{
    public class CategoryServiceTest
    {
        private readonly ICategoryService _mockCategoryService;
        private Mock<IValueChainService> _valueChainServiceMock;
        private readonly IChartService _mockChartService;
        private ConnectionFactory _connectionFactory = new ConnectionFactory();

        public CategoryServiceTest()
        {
            _valueChainServiceMock = new Mock<IValueChainService>();
            _mockChartService = new ChartService(_connectionFactory.CreateContextForInMemory());
            _mockCategoryService = new CategoryService(_valueChainServiceMock.Object);
            var context = _connectionFactory.CreateContextForInMemory();
            var entityTypes = new List<EntityType>
             {
                 new EntityType
                 {
                     Id = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     Name = "MTBE",
                     EntityGroupName = "Plant",
                     EntityId = 405,
                     FullName = "Methyl Tertiary Butyl Ether",

                 },
                 new EntityType
                 {
                     Id = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     Name = "BA",
                     EntityGroupName = "Plant",
                     EntityId = 436,
                     FullName = "Butyl Acetate",

                 },
                 new EntityType
                 {
                     Id = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
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
                    Id = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                    Name = "Gas chain",
                    Type = ChainType.Gas
                },
                new DigitalTwin.Data.Entities.ValueChainType
                {
                    Id = Guid.Parse("8d6cd6e6-df8f-7406-c75c-d14c7315ff73"),
                    Name = "Oil chain",
                    Type = ChainType.Oil
                }
             };
            context.ValueChainTypes?.AddRange(valueChainTypeData);

            var uom = new List<UnitOfMeasure>
             {
                 new UnitOfMeasure
                 {
                     Name = "mmscfd",
                     Id = Guid.Parse("14e17760-78cf-9d23-3595-f860150a8aaf"),
                 },
                 new UnitOfMeasure
                 {
                     Name = "kbpd",
                     Id = Guid.Parse("7cefef12-0054-2df3-d2a7-c1122c35dc49"),
                 },
             };
            context.UnitOfMeasures?.AddRange(uom);

            var entities = new List<DigitalTwin.Data.Entities.Entity>
             {
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                     Id = Guid.NewGuid(),
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                     Id = Guid.NewGuid(),
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 436,
                     Depth = 3,
                     Name = "BA",
                     KpiPath = "Downstream > PCGB > PCD > BA"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                     Id = Guid.NewGuid(),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId =  Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
                 new DigitalTwin.Data.Entities.Entity
                 {
                     ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                     Id = Guid.NewGuid(),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId =  Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     Depth = 3,
                     Name = "MTBE",
                     KpiPath = "Downstream > PCGB > MTBE"
                 },
             };
            context.Entities?.AddRange(entities);


            var products = new List<ProductLink>
             {
                 new ProductLink
                 {
                     Id = Guid.Parse("0a4d5087-b8d6-e456-80e0-0d8948f32d23"),
                     UnitOfMeasureId = Guid.Parse("14e17760-78cf-9d23-3595-f860150a8aaf"),
                     EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     FullName = "Production Volume",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("73a652da-1a73-cc0b-5630-4a6e81e4fe71"),
                     UnitOfMeasureId = Guid.Parse("14e17760-78cf-9d23-3595-f860150a8aaf"),
                     EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     FullName = "Plant Utilization",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("bc591932-478c-1420-f09b-67348b765f96"),
                     UnitOfMeasureId = Guid.Parse("14e17760-78cf-9d23-3595-f860150a8aaf"),
                     EntityMapId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Name = "Plant Utilization",
                     FullName = "Plant Utilization",
                 },
                 new ProductLink
                 {
                     Id = Guid.Parse("4a75b07d-0637-aeaa-623d-ab719fed706d"),
                     UnitOfMeasureId = Guid.Parse("14e17760-78cf-9d23-3595-f860150a8aaf"),
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
                     ProductLinkId = Guid.Parse("0a4d5087-b8d6-e456-80e0-0d8948f32d23"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color ="red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                     NumValues =  "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("73a652da-1a73-cc0b-5630-4a6e81e4fe71"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color ="red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues = "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("bc591932-478c-1420-f09b-67348b765f96"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color ="red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues = "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("4a75b07d-0637-aeaa-623d-ab719fed706d"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2021-06-16"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "%",
                     Color ="red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues = "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
             };
            context.ProductLinkDetails?.AddRange(productLinkDetails);
            context.SaveChanges();
        }

        /*[Fact]
        public async Task GetValueChain_Success()
        {
            var request = new GetCategoryRequest
            {
                ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsRealTime = false,
                IsHour = false,
                PageSize = 10,
                PageNumber = 0,
                Category = "Customer"
            };

            var listEntities = new List<EntityDto>
             {
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Customer",
                     Alias = "mtbe",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 436,
                     Name = "BA",
                     Type = "BA",
                     Category = "Customer",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Customer",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Customer",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                 },
                     }
                 },
             };

            var listProducts = new List<ProductDto>
             {
                 new ProductDto
                 {
                     Id = Guid.Parse("0a4d5087-b8d6-e456-80e0-0d8948f32d23"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     Alias = "production volume",
                     Category = "Customer",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                   },
                 new ProductDto
                 {
                     Id = Guid.Parse("73a652da-1a73-cc0b-5630-4a6e81e4fe71"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Customer",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 436,
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("bc591932-478c-1420-f09b-67348b765f96"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Customer",
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("4a75b07d-0637-aeaa-623d-ab719fed706d"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Customer",
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
             };

            _valueChainServiceMock.Setup(p => p.GetValueChains(It.IsAny<ValueChainRequest>(), default))
                .ReturnsAsync(new ValueChainResponse
                {
                    Title = "Gas Chain",
                    Entities = listEntities,
                    Products = listProducts
                });

            var result = await _mockCategoryService.GetCategories(request, default);
            Assert.True(result?.Result?.Count > 0);
        }
        [Fact]
        public async Task GetValueChainForBussinessCategory_Success()
        {
            var request = new GetCategoryRequest
            {
                ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsRealTime = false,
                IsHour = false,
                PageSize = 10,
                PageNumber = 0,
                Category = "Business"
            };

            var listEntities = new List<EntityDto>
             {
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Plant",
                     Alias = "mtbe",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Category = "Plant",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Plant",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 436,
                     Name = "BA",
                     Type = "BA",
                     Category = "Plant",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Category = "Plant",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Plant",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Plant",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Plant",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Plant",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Plant",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Category = "Plant",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Plant",
                 },
                     }
                 },
             };

            var listProducts = new List<ProductDto>
             {
                 new ProductDto
                 {
                     Id = Guid.Parse("0a4d5087-b8d6-e456-80e0-0d8948f32d23"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     Alias = "production volume",
                     Category = "Plant",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                   },
                 new ProductDto
                 {
                     Id = Guid.Parse("73a652da-1a73-cc0b-5630-4a6e81e4fe71"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Plant",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 436,
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("bc591932-478c-1420-f09b-67348b765f96"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Plant",
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("4a75b07d-0637-aeaa-623d-ab719fed706d"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Plant",
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
             };

            _valueChainServiceMock.Setup(p => p.GetValueChains(It.IsAny<ValueChainRequest>(), default))
                .ReturnsAsync(new ValueChainResponse
                {
                    Title = "Gas Chain",
                    Entities = listEntities,
                    Products = listProducts
                });

            var result = await _mockCategoryService.GetCategories(request, default);
            Assert.True(result?.Result?.Count > 0);
        }*/
        [Fact]
        public async Task GetValueChainForProductCategory_Success()
        {
            var request = new GetCategoryRequest
            {
                ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsRealTime = false,
                IsHour = false,
                PageSize = 10,
                PageNumber = 0,
                Category = "Product"
            };

            var listEntities = new List<EntityDto>
             {
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Product",
                     Alias = "mtbe",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Category = "Product",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Product",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 436,
                     Name = "BA",
                     Type = "BA",
                     Category = "Product",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Category = "Product",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Product",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Product",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Product",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Product",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Product",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Category = "Product",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Product",
                 },
                     }
                 },
             };

            var listProducts = new List<ProductDto>
             {
                 new ProductDto
                 {
                     Id = Guid.Parse("0a4d5087-b8d6-e456-80e0-0d8948f32d23"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     Alias = "production volume",
                     Category = "Product",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                   },
                 new ProductDto
                 {
                     Id = Guid.Parse("73a652da-1a73-cc0b-5630-4a6e81e4fe71"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Product",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 436,
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("bc591932-478c-1420-f09b-67348b765f96"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Product",
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("4a75b07d-0637-aeaa-623d-ab719fed706d"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Product",
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
             };

            _valueChainServiceMock.Setup(p => p.GetValueChains(It.IsAny<ValueChainRequest>(), default))
                .ReturnsAsync(new ValueChainResponse
                {
                    Title = "Gas Chain",
                    Entities = listEntities,
                    Products = listProducts
                });

            var result = await _mockCategoryService.GetCategories(request, default);
            Assert.True(result?.Result?.Count > 0);
        }
        /*[Fact]
        public async Task GetValueChainNo_Success()
        {
            var request = new GetCategoryRequest
            {
                ValueChainTypeId = Guid.Parse("bddaf339-3750-f9f0-7258-72ec7173e00b"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsRealTime = false,
                IsHour = false,
                PageSize = 10,
                PageNumber = 0,
                Category = "Platform"
            };

            var listEntities = new List<EntityDto>
             {
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     Name = "MTBE",
                     Type = "MTBE",
                     Category = "Platform",
                     Alias = "mtbe",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Category = "Platform",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Platform",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 436,
                     Name = "BA",
                     Type = "BA",
                     Category = "Platform",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Category = "Platform",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Platform",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Platform",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Platform",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Platform",
                 },
                     }
                 },
                 new EntityDto
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     Name = "BA",
                     Type = "BA",
                     Category = "Platform",
                     Alias = "ba",
                     ChildrenList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Category = "Platform",
                 },
                     },
                     ParentList = new List<DigitalTwin.Models.Responses.ValueChain.Entity>()
                     {
                         new DigitalTwin.Models.Responses.ValueChain.Entity
                 {
                     Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Category = "Platform",
                 },
                     }
                 },
             };

            var listProducts = new List<ProductDto>
             {
                 new ProductDto
                 {
                     Id = Guid.Parse("0a4d5087-b8d6-e456-80e0-0d8948f32d23"),
                     EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                     Name = "Production Volume",
                     Alias = "production volume",
                     Category = "Platform",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     EntityTypeMasterId = 405,
                     EntityTypeId = Guid.Parse("4a1038ca-3101-c0cb-2117-b41ef8dbcb3e"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                   },
                 new ProductDto
                 {
                     Id = Guid.Parse("73a652da-1a73-cc0b-5630-4a6e81e4fe71"),
                     EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Platform",
                     EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
                     EntityTypeMasterId = 436,
                     EntityTypeId = Guid.Parse("1f1ac811-d453-ada7-b63b-5031dd48a40c"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("bc591932-478c-1420-f09b-67348b765f96"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Platform",
                     EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
                 new ProductDto
                 {
                     Id = Guid.Parse("4a75b07d-0637-aeaa-623d-ab719fed706d"),
                     EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
                     Name = "Plant Utilization",
                     Alias = "plant utilization",
                     Category = "Platform",
                     EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                     EntityTypeMasterId = 401,
                     EntityTypeId = Guid.Parse("d2dee3f8-5237-b25f-726a-7cbfef67911b"),
                     Parent = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                     Color ="red",
                     Value = 34,
                     Variance = 34,
                     Percentage = 30,
                 },
             };

            _valueChainServiceMock.Setup(p => p.GetValueChains(It.IsAny<ValueChainRequest>(), default))
                .ReturnsAsync(new ValueChainResponse
                {
                    Title = "Gas Chain",
                    Entities = listEntities,
                    Products = listProducts
                });

            var result = await _mockCategoryService.GetCategories(request, default);
            Assert.True(result?.Result?.Count > 0);
        }*/
    }
}
