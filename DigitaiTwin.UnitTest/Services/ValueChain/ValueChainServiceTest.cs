/*using System.Data;
using DigitalTwin.Business.Helpers;
using DigitalTwin.Business.Services.Chart;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Data.Database;
using DigitalTwin.Data.Entities;
using DigitalTwin.Models.DTOs;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;

namespace DigitaiTwin.UnitTest.Services.ValueChain
{
    public class ValueChainServiceTest
    {
        private readonly IValueChainService _mockValueChainService;
        private readonly IChartService _mockChartService;
        private ConnectionFactory _connectionFactory = new();
        private DigitalTwinContext _context;
        private readonly Mock<IReadResultHelper> _mockReadResultHelper;

        public ValueChainServiceTest()
        {
            _context = _connectionFactory.CreateContextForSQLite();
            _mockChartService = new ChartService(_context);
            _mockReadResultHelper = new Mock<IReadResultHelper>();
            _mockValueChainService =
                new ValueChainService(_context, _mockChartService!, _mockReadResultHelper.Object);

            DummyDefaultData();
        }

        [Fact]
        public async Task GetValueChain_Success()
        {
            var entities = new List<DigitalTwin.Data.Entities.Entity>
                  {
                      new()
                      {
                          ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                          Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813c"),
                          EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                          EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                          EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                          EntityTypeMasterId = 405,
                          LevelId = 8,
                          Density = "Product",
                          Depth = 3,
                          Name = "MTBE",
                          KpiPath = "Downstream > PCGB > MTBE"
                      },
                      new()
                      {
                          ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                          Id = Guid.Parse("bfd01884-39ea-1338-24c0-e1fc52c83460"),
                          EntityTypeId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                          EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                          EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                          EntityTypeMasterId = 436,
                          LevelId = 8,
                          Density = "Product",
                          Depth = 3,
                          Name = "BA",
                          KpiPath = "Downstream > PCGB > PCD > BA"
                      },
                  };
            _context.Entities?.AddRange(entities);

            var products = new List<ProductLink>
                  {
                      new()
                      {
                          Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                          UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                          EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                          Name = "Production Volume",
                          FullName = "Production Volume",
                          LevelId = 8,
                          LevelName = "Product"
                      },
                      new()
                      {
                          Id = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
                          UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                          EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                          Name = "Production Volume",
                          FullName = "Production Volume",
                          LevelId = 8,
                          LevelName = "Product"
                      },
                  };
            _context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
                  {
                      new()
                      {
                          Id = Guid.NewGuid(),
                          ProductLinkId = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
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
                      new()
                      {
                          Id = Guid.NewGuid(),
                          ProductLinkId = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
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

            _context.ProductLinkDetails?.AddRange(productLinkDetails);
            await _context.SaveChangesAsync();

            var request = new ValueChainRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsFilter = false,
                Entities = new List<Guid>()
            };


            var queryResponse = new List<DataProductDto>
                {
                    new DataProductDto
                    {
                        Id = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    }
                };

            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(queryResponse);
            var result = await _mockValueChainService.GetValueChainByType(request, default);
            Assert.IsType<Response<ValueChainResponse>>(result);

        }

        [Fact]
        public async Task GetValueChainDetailNull_Success()
        {
            var request = new ViewBusinessDashboardRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsBusiness = false,
                IsFilter = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsRealTime = false,
                IsWeekly = false,
                EntityId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19")
            };

            var queryResponse = new List<DataProductDto>
                {
                    new DataProductDto
                    {
                        Id = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("dfe55e8e-3c52-4b37-9708-dd097eca6918"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    }
                };

            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(queryResponse);
            var result = await _mockValueChainService.GetValueChainDetail(request, default);
            Assert.Null(result.Result!.Data);

        }

        *//*[Fact]
        public async Task GetValueChainHaveParent_Success()
        {
            var entities = new List<DigitalTwin.Data.Entities.Entity>
              {
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "KPBI",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > KPBI"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                      EntityParentId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "BA",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("5dfae56d-b41a-4e13-b322-a80ec4b068e0"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Customer",
                      Depth = 3,
                      Name = "PCS",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > PCS"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("69242fbb-4077-463b-be31-84325686a679"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "MTBE",
                      KpiPath = "Downstream > PCGB > BA > MTBE"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      EntityParentId = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeMasterId = 436,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "PCD",
                      KpiPath = "Downstream > PCGB > MTBE > PCD"
                  }
              };
            _context.Entities?.AddRange(entities);

            var products = new List<ProductLink>
              {
                  new()
                  {
                      Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  }
              };
            _context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
              {
                  new()
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
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
            _context.ProductLinkDetails?.AddRange(productLinkDetails);
            _context.SaveChanges();

            var request = new ValueChainRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsFilter = false,
                Entities = new List<Guid>()
            };

            var queryResponse = new List<DataProductDto>
                {
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                        EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                        EntityMapId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                        EntityMapId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("674c08ee-e6e3-4183-9119-ed4031d77ccc"),
                        EntityMapId = Guid.Parse("69242fbb-4077-463b-be31-84325686a679"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    }
                };

            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(queryResponse);
            var result = await _mockValueChainService.GetValueChainByType(request, default);
            Assert.True(!string.IsNullOrEmpty(result.Result?.Title));
            Assert.True(result.Result?.Entities!.Count >= 1);
            Assert.True(result.Result?.Products!.Count >= 1);
        }*//*

        //[Fact]
        //public async Task GetAllKPI_Success()
        //{
        //    var entities = new List<DigitalTwin.Data.Entities.Entity>
        //      {
        //          new()
        //          {
        //              ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
        //              Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813c"),
        //              EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
        //              EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
        //              EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
        //              EntityTypeMasterId = 405,
        //              LevelId = 8,
        //              Density = "Product",
        //              Depth = 3,
        //              Name = "MTBE",
        //              KpiPath = "Downstream > PCGB > MTBE"
        //          },
        //          new()
        //          {
        //              ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
        //              Id = Guid.Parse("bfd01884-39ea-1338-24c0-e1fc52c83460"),
        //              EntityTypeId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
        //              EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
        //              EntityParentId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
        //              EntityTypeMasterId = 436,
        //              LevelId = 8,
        //              Density = "Product",
        //              Depth = 3,
        //              Name = "BA",
        //              KpiPath = "Downstream > PCGB > PCD > BA"
        //          },
        //          new()
        //          {
        //              ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
        //              Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813d"),
        //              EntityTypeId = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
        //              EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
        //              EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d1"),
        //              EntityTypeMasterId = 401,
        //              LevelId = 8,
        //              Density = "Product",
        //              Depth = 3,
        //              Name = "MTBE",
        //              KpiPath = "Downstream > PCGB > MTBE"
        //          },
        //          new()
        //          {
        //              ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
        //              Id = Guid.Parse("ce0c4b87-986a-7e56-8d6a-f84adee0813f"),
        //              EntityTypeId = Guid.Parse("cc044aeb-2613-cf12-8ebc-af14e83cefb4"),
        //              EntityId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
        //              EntityParentId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
        //              EntityTypeMasterId = 401,
        //              LevelId = 8,
        //              Density = "Product",
        //              Depth = 3,
        //              Name = "MTBE",
        //              KpiPath = "Downstream > PCGB > MTBE"
        //          },
        //      };
        //    _context.Entities?.AddRange(entities);

        //    var products = new List<ProductLink>
        //      {
        //          new()
        //          {
        //              Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
        //              UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
        //              EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
        //              Name = "Production Volume",
        //              FullName = "Production Volume",
        //              LevelId = 8,
        //              LevelName = "Product"
        //          },
        //          new()
        //          {
        //              Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
        //              UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
        //              EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
        //              Name = "Production Volume",
        //              FullName = "Production Volume",
        //              LevelId = 8,
        //              LevelName = "Product"
        //          },
        //          new()
        //          {
        //              Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
        //              UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
        //              EntityMapId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
        //              Name = "Production Volume",
        //              FullName = "Production Volume",
        //              LevelId = 8,
        //              LevelName = "Product"
        //          },
        //          new ProductLink
        //          {
        //              Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49921"),
        //              UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
        //              EntityMapId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
        //              Name = "Production Volume",
        //              FullName = "Production Volume",
        //              LevelId = 8,
        //              LevelName = "Product"
        //          },
        //      };
        //    _context.ProductLinks?.AddRange(products);

        //    var productLinkDetails = new List<ProductLinkDetail>
        //      {
        //          new ProductLinkDetail
        //          {
        //              Id = Guid.NewGuid(),
        //              ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
        //              Frequency = "monthly",
        //              DataDate = DateTime.Parse("2021-06-16"),
        //              IsDaily = true,
        //              IsMonthly = true,
        //              IsMonthToDate = false,
        //              IsRealTime = false,
        //              UomName = "%",
        //              Color = "red",
        //              NorCode = "4",
        //              Value = 34,
        //              Variance = 34,
        //              Percentage = 30,
        //              NumValues =
        //                  "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
        //          },
        //          new()
        //          {
        //              Id = Guid.NewGuid(),
        //              ProductLinkId = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
        //              Frequency = "monthly",
        //              DataDate = DateTime.Parse("2021-06-16"),
        //              IsDaily = true,
        //              IsMonthly = true,
        //              IsMonthToDate = false,
        //              IsRealTime = false,
        //              UomName = "%",
        //              Color = "red",
        //              NorCode = "4",
        //              Value = 34,
        //              Variance = 34,
        //              Percentage = 50,
        //              NumValues =
        //                  "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
        //          },
        //          new()
        //          {
        //              Id = Guid.NewGuid(),
        //              ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
        //              Frequency = "monthly",
        //              DataDate = DateTime.Parse("2021-06-16"),
        //              IsDaily = true,
        //              IsMonthly = true,
        //              IsMonthToDate = false,
        //              IsRealTime = false,
        //              UomName = "%",
        //              Color = "red",
        //              NorCode = "4",
        //              Value = 34,
        //              Variance = 34,
        //              Percentage = 50,
        //              NumValues =
        //                  "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
        //          },
        //          new()
        //          {
        //              Id = Guid.NewGuid(),
        //              ProductLinkId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d9"),
        //              Frequency = "monthly",
        //              DataDate = DateTime.Parse("2021-06-16"),
        //              IsDaily = true,
        //              IsMonthly = true,
        //              IsMonthToDate = false,
        //              IsRealTime = false,
        //              UomName = "%",
        //              Color = "red",
        //              NorCode = "4",
        //              Value = 34,
        //              Variance = 34,
        //              Percentage = 50,
        //              NumValues =
        //                  "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
        //          },
        //      };
        //    _context.ProductLinkDetails?.AddRange(productLinkDetails);

        //    _context.SaveChanges();

        //    var request = new GetAllKPIRequest
        //    {
        //        ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
        //        FromDate = DateTime.Parse("2021-06-01"),
        //        ToDate = DateTime.Parse("2021-06-30"),
        //        IsDaily = false,
        //        IsMonthly = true,
        //        IsMonthToDate = false,
        //        IsQuarterly = false,
        //        IsYearToDaily = false,
        //        IsYearToMonthly = false,
        //        IsYearEndProjection = false,
        //        IsHour = false,
        //        IsQuarterToDate = false,
        //        IsRealTime = false,
        //        IsWeekly = false,
        //    };

        //    var queryResponse = new List<DataProductDto>
        //        {
        //            new DataProductDto
        //            {
        //                Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
        //                EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
        //                Name = "Production Volume",
        //                Unit = "mmscfd",
        //                NumValues =
        //                     "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
        //                Color = "red",
        //                EntityGroupName = null,
        //                KpiPath = null,
        //                EntityId = null
        //            },
        //            new DataProductDto
        //            {
        //                Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
        //                EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
        //                Name = "Production Volume",
        //                Unit = "mmscfd",
        //                NumValues =
        //                     "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
        //                Color = "red",
        //                EntityGroupName = null,
        //                KpiPath = null,
        //                EntityId = null
        //            },
        //            new DataProductDto
        //            {
        //                Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
        //                EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
        //                Name = "Production Volume",
        //                Unit = "mmscfd",
        //                NumValues =
        //                      "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
        //                Color = "red",
        //                EntityGroupName = null,
        //                KpiPath = null,
        //                EntityId = null
        //            },
        //            new DataProductDto
        //            {
        //                Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
        //                EntityMapId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
        //                Name = "Production Volume",
        //                Unit = "mmscfd",
        //                NumValues =
        //                      "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
        //                Color = "red",
        //                EntityGroupName = null,
        //                KpiPath = null,
        //                EntityId = null
        //            }
        //        };

        //    var x = _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
        //        .ReturnsAsync(queryResponse);
        //    var result = await _mockValueChainService.GetAllKPI(request, default);
        //    Assert.True(result.Result?.ProductList?.Count >= 1);
        //}

        [Fact]
        public async Task GetAllFrequency_Success()
        {
            var productLinkDetails = new List<ProductLinkDetail>
              {
                  new ProductLinkDetail
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                      Frequency = "ytd",
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                      Frequency = "hourly",
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                      Frequency = "daily",
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
                  new()
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
            _context.ProductLinkDetails?.AddRange(productLinkDetails);

            _context.SaveChanges();

            var result = await _mockValueChainService.GetFrequencies(default);
            Assert.True(result.Result?.Frequencies?.Count >= 0);
        }

        [Fact]
       *//* public async Task GetValueChainDetail_Success()
        {
            var entities = new List<DigitalTwin.Data.Entities.Entity>
              {
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "KPBI",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > KPBI"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                      EntityParentId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "BA",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("5dfae56d-b41a-4e13-b322-a80ec4b068e0"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Customer",
                      Depth = 3,
                      Name = "PCS",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > PCS"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("69242fbb-4077-463b-be31-84325686a679"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "MTBE",
                      KpiPath = "Downstream > PCGB > MTBE"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      EntityParentId = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeMasterId = 436,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "PCD",
                      KpiPath = "Downstream > PCGB > MTBE > PCD"
                  }
              };
            _context.Entities?.AddRange(entities);

            var products = new List<ProductLink>
              {
                  new()
                  {
                      Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  }
              };
            _context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
              {
                  new()
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
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
            _context.ProductLinkDetails?.AddRange(productLinkDetails);
            _context.SaveChanges();

            var request = new ViewBusinessDashboardRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsBusiness = false,
                IsFilter = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsRealTime = false,
                IsWeekly = false,
                EntityId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19")
            };

            var queryResponse = new List<DataProductDto>
                {
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                     new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Sales Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = "Customer",
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                        EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                        EntityMapId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    }
                };
            var semDataResponse = new List<SemDataDto>
            {
                new SemDataDto
                {
                    Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    SemDataId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    PathStreamId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    KpiId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    DataDate = DateTime.Parse("2021-06-16"),
                    UomName = "",
                    ActualNum = 3123,
                    TargetNum = 2234,
                    VariancePercentage = 312434,
                    Justification = "",
                    Name = "",
                    Updated_At = DateTime.Parse("2021-06-30")
                }
            };

            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(queryResponse);
            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<SemDataDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(semDataResponse);
            var result = await _mockValueChainService.GetValueChainDetail(request, default);
            Assert.NotNull((result.Result?.Data));
        }*//*

        [Theory]
        [InlineData("red")]
        [InlineData("green")]
        [InlineData("yellow")]
        [InlineData("white")]
        public async Task GetValueChainDetailBusiness_Success(string color)
        {
            var entities = new List<DigitalTwin.Data.Entities.Entity>
              {
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "KPBI",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > KPBI"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                      EntityParentId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "BA",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("5dfae56d-b41a-4e13-b322-a80ec4b068e0"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Customer",
                      Depth = 3,
                      Name = "PCS",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > PCS"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("69242fbb-4077-463b-be31-84325686a679"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "MTBE",
                      KpiPath = "Downstream > PCGB > MTBE"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      EntityParentId = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeMasterId = 436,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "PCD",
                      KpiPath = "Downstream > PCGB > MTBE > PCD"
                  }
              };
            _context.Entities?.AddRange(entities);

            var products = new List<ProductLink>
              {
                  new()
                  {
                      Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  }
              };
            _context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
              {
                  new()
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
                      Color = color,
                      NorCode = "4",
                      Value = 34,
                      Variance = 34,
                      Percentage = 30,
                      NumValues =
                          "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                  },
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                      Frequency = "monthly",
                      DataDate = DateTime.Parse("2021-06-16"),
                      IsDaily = true,
                      IsMonthly = true,
                      IsMonthToDate = false,
                      IsRealTime = false,
                      UomName = "%",
                      Color = color,
                      NorCode = "4",
                      Value = 34,
                      Variance = 34,
                      Percentage = 50,
                      NumValues =
                          "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                  },
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                      Frequency = "monthly",
                      DataDate = DateTime.Parse("2021-06-16"),
                      IsDaily = true,
                      IsMonthly = true,
                      IsMonthToDate = false,
                      IsRealTime = false,
                      UomName = "%",
                      Color = color,
                      NorCode = "4",
                      Value = 34,
                      Variance = 34,
                      Percentage = 50,
                      NumValues =
                          "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                  },
              };
            _context.ProductLinkDetails?.AddRange(productLinkDetails);
            _context.SaveChanges();

            var request = new ViewBusinessDashboardRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsBusiness = true,
                IsFilter = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsRealTime = false,
                IsWeekly = false,
                EntityId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19")
            };

            var queryResponse = new List<DataProductDto>
                {
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = color,
                        EntityGroupName = "Plant",
                        KpiPath = "Downstream > PCGB > MTBE > PCD",
                        EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe")
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = color,
                        EntityGroupName = "Plant",
                        KpiPath = "Downstream > PCGB > MTBE > PCD",
                        EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe")
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                        EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = color,
                        EntityGroupName = "Plant",
                        KpiPath = "Downstream > PCGB > MTBE > PCD > BA > KPBI",
                        EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136")
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                        EntityMapId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                        Name = "Throughput",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = color,
                        EntityGroupName = "Plant",
                        KpiPath = "Downstream > PCGB > MTBE > PCD > BA",
                        EntityId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                    }
                };
            var semDataResponse = new List<SemDataDto>
            {
                new SemDataDto
                {
                    Id = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                    SemDataId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    PathStreamId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    KpiId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    DataDate = DateTime.Parse("2021-06-16"),
                    UomName = "mmscfd",
                    ActualNum = 3123,
                    TargetNum = 2234,
                    VariancePercentage = 312434,
                    Justification = "Justification",
                    Name = "Throughput",
                    Updated_At = DateTime.Parse("2021-06-30")
                }
            };
            var nameDataResponse = new List<NameDataDto>
            {
                new NameDataDto
                {
                    DisplayName = "Throughput"
                }
            };

            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(queryResponse);
            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<SemDataDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(semDataResponse);
            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<SemDataDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(semDataResponse);
            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<NameDataDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(nameDataResponse);
            var result = await _mockValueChainService.GetValueChainDetail(request, default);
            Assert.NotNull((result.Result?.Data));
        }


        [Fact]
        *//*public async Task GetProductDetail_Success()
        {
            var entities = new List<DigitalTwin.Data.Entities.Entity>
              {
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "KPBI",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > KPBI"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                      EntityParentId = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "BA",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("5dfae56d-b41a-4e13-b322-a80ec4b068e0"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Customer",
                      Depth = 3,
                      Name = "PCS",
                      KpiPath = "Downstream > PCGB > MTBE > PCD > BA > PCS"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("69242fbb-4077-463b-be31-84325686a679"),
                      EntityParentId = Guid.Parse("eeda66d2-f393-40dc-9380-7dfb9abbd2d8"),
                      EntityTypeMasterId = 405,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "MTBE",
                      KpiPath = "Downstream > PCGB > MTBE"
                  },
                  new()
                  {
                      ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Id = Guid.Parse("760c03fb-d30e-437a-b243-569c5fda2c19"),
                      EntityTypeId = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      EntityId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      EntityParentId = Guid.Parse("b7041da0-0d4f-40cf-821f-d9aec2665290"),
                      EntityTypeMasterId = 436,
                      LevelId = 8,
                      Density = "Product",
                      Depth = 3,
                      Name = "PCD",
                      KpiPath = "Downstream > PCGB > MTBE > PCD"
                  }
              };
            _context.Entities?.AddRange(entities);

            var products = new List<ProductLink>
              {
                  new()
                  {
                      Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f4992b"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  },
                  new()
                  {
                      Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                      UnitOfMeasureId = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                      EntityMapId = Guid.Parse("c6beeff0-cd6a-4f3b-8fdb-ed1d935ca978"),
                      Name = "Production Volume",
                      FullName = "Production Volume",
                      LevelId = 8,
                      LevelName = "Product"
                  }
              };
            _context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
              {
                  new()
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
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
                  new()
                  {
                      Id = Guid.NewGuid(),
                      ProductLinkId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
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
            _context.ProductLinkDetails?.AddRange(productLinkDetails);
            _context.SaveChanges();

            var request = new GetProductDetailRequest
            {
                ValueChainTypeId = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                FromDate = DateTime.Parse("2021-06-01"),
                ToDate = DateTime.Parse("2021-06-30"),
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false,
                IsFilter = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsRealTime = false,
                IsWeekly = false,
                ProductId = Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba"),
                ProductName = "Production Volume"
            };

            var queryResponse = new List<DataProductDto>
                {
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                     new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Sales Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                        EntityMapId = Guid.Parse("74022c47-7dcf-4c25-b4be-b526517a6afe"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                             "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("0010bbe8-5427-4964-99d9-d54a8a7b1026"),
                        EntityMapId = Guid.Parse("565001f7-2c8f-4fc9-92e5-cddf574f9136"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    },
                    new DataProductDto
                    {
                        Id = Guid.Parse("ae190adf-3343-497b-91ca-2b906927bb0d"),
                        EntityMapId = Guid.Parse("cc93652e-f99b-4589-8ee0-d04d996cab2c"),
                        Name = "Production Volume",
                        Unit = "mmscfd",
                        NumValues =
                              "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}",
                        Color = "red",
                        EntityGroupName = null,
                        KpiPath = null,
                        EntityId = null
                    }
                };
            var semDataResponse = new List<SemDataDto>
            {
                new SemDataDto
                {
                    Id = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    SemDataId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    PathStreamId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    KpiId = Guid.Parse("de07f4b4-0798-40fb-a5f4-90ffc1f49929"),
                    DataDate = DateTime.Parse("2021-06-16"),
                    UomName = "",
                    ActualNum = 3123,
                    TargetNum = 2234,
                    VariancePercentage = 312434,
                    Justification = "",
                    Name = "",
                    Updated_At = DateTime.Parse("2021-06-30")
                }
            };

            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<DataProductDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(queryResponse);
            _mockReadResultHelper.Setup(_ => _.ExecuteResultFromQueryAsync<SemDataDto>(It.IsAny<DigitalTwinContext>(), It.IsAny<string>(), default))
                .ReturnsAsync(semDataResponse);
            var result = await _mockValueChainService.GetProductDetail(request, default);
            Assert.NotNull((result.Result?.Data));
        }*//*

        private void DummyDefaultData()
        {
            var entityTypes = new List<EntityType>
              {
                  new()
                  {
                      Id = Guid.Parse("09623604-f03c-4f60-958e-dd8368537016"),
                      Name = "MTBE",
                      EntityGroupName = "Plant",
                      EntityId = 405,
                      FullName = "Methyl Tertiary Butyl Ether",
                  },
                  new()
                  {
                      Id = Guid.Parse("7c58f972-eb21-4bff-8e7e-78d644f06263"),
                      Name = "BA",
                      EntityGroupName = "Platform",
                      EntityId = 436,
                      FullName = "Butyl Acetate",
                  },
                  new()
                  {
                      Id = Guid.Parse("cc044aeb-2613-cf12-8ebc-af14e83cefb4"),
                      Name = "BA",
                      EntityGroupName = "Customer",
                      EntityId = 436,
                      FullName = "Butyl Acetate",
                  },
                  new()
                  {
                      Id = Guid.Parse("fd8ca29d-65a4-6f0b-2ba1-d6f0e6cb6578"),
                      Name = "BA",
                      EntityGroupName = "Product",
                      EntityId = 436,
                      FullName = "Butyl Acetate",
                  },
              };
            _context.EntityTypes?.AddRange(entityTypes);

            var valueChainTypeData = new List<DigitalTwin.Data.Entities.ValueChainType>
              {
                  new()
                  {
                      Id = Guid.Parse("fb3f44d0-dbb9-4994-87ec-8cdfab8f52a0"),
                      Name = "Gas chain",
                      Type = ChainType.Gas
                  },
                  new()
                  {
                      Id = Guid.Parse("4de5c469-90e9-4d19-ab12-674bb69efe38"),
                      Name = "Oil chain",
                      Type = ChainType.Oil
                  }
              };
            _context.ValueChainTypes?.AddRange(valueChainTypeData);

            var uom = new List<UnitOfMeasure>
              {
                  new()
                  {
                      Name = "mmscfd",
                      Id = Guid.Parse("a6b30a96-9b62-49d6-be13-14ee900e721b"),
                  },
                  new()
                  {
                      Name = "kbpd",
                      Id = Guid.Parse("5f8eeeba-bdf2-4ea0-aae6-cf0659fcca5c"),
                  },
              };
            _context.UnitOfMeasures?.AddRange(uom);
            _context.SaveChanges();
        }
    }
}*/