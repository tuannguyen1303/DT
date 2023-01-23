using DigitalTwin.Business.Services.Chart;
using DigitalTwin.Business.Services.Entity;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Common.Constants;
using DigitalTwin.Data.Database;
using DigitalTwin.Data.Entities;
using DigitalTwin.Models.Requests.Chart;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Responses.Chart;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Services.Chart
{
    public class ChartServiceTest
    {
        private IChartService _mockChartService;
        private DigitalTwinContext context;
        private ConnectionFactory _connectionFactory = new ConnectionFactory();

        public ChartServiceTest()
        {
            _mockChartService = new ChartService(_connectionFactory.CreateContextForInMemory());
            context = _connectionFactory.CreateContextForInMemory();
            var entities = new List<DigitalTwin.Data.Entities.Entity>
             {
                 new()
                 {
                     ValueChainTypeId = Guid.Parse("ab0d6efa-c806-8492-09b1-2a004afb9b2a"),
                     Id = Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba"),
                     EntityTypeId = Guid.Parse("49d33212-8598-4ecc-9709-dfa28522eaa0"),
                     EntityId = Guid.Parse("5dfae56d-b41a-4e13-b322-a80ec4b068e0"),
                     EntityParentId = Guid.Parse("eabbf2e4-ba36-427c-95b3-21056cd267b6"),
                     EntityTypeMasterId = 424,
                     LevelId = 8,
                     Density = "Product",
                     Depth = 3,
                     Name = "Isobutylene",
                     KpiPath = "Downstream > PCGB > PCMTBE > MTBE > Isobutylene"
                 }
             };
            context.Entities?.AddRange(entities);

            var products = new List<ProductLink>
             {
                 new ProductLink
                 {
                     Id = Guid.Parse("da395715-fd95-43a5-a668-f8e8adb642db"),
                     UnitOfMeasureId = Guid.Parse("25c23226-2173-4235-9d3b-ffca9eeea649"),
                     EntityMapId = Guid.Parse("5dfae56d-b41a-4e13-b322-a80ec4b068e0"),
                     Name = "Production Volume",
                     FullName = "Production Volume",
                     LevelId = 8,
                     LevelName = "Product"
                 },
             };
            context.ProductLinks?.AddRange(products);

            var productLinkDetails = new List<ProductLinkDetail>
             {
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("da395715-fd95-43a5-a668-f8e8adb642db"),
                     Frequency = "monthly",
                     DataDate = DateTime.Parse("2022-05-31"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "kMT",
                     Color ="green",
                     NorCode = "2",
                     Value = (decimal?)4.5528380000000000,
                     Variance = (decimal?)0.6361380000000000,
                     Percentage = (decimal?)16.24168304950596165100,
                     NumValues =  "{\r\n\"actual\":4.5528380000000000,\r\n\"_target\":75,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
                 new ProductLinkDetail
                 {
                     Id = Guid.NewGuid(),
                     ProductLinkId = Guid.Parse("da395715-fd95-43a5-a668-f8e8adb642db"),
                     Frequency = "daily",
                     DataDate = DateTime.Parse("2022-11-30"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "kMT",
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
                     ProductLinkId = Guid.Parse("da395715-fd95-43a5-a668-f8e8adb642db"),
                     Frequency = "hourly",
                     DataDate = DateTime.Parse("2022-11-27 16:59:59"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "KMT",
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
                     ProductLinkId = Guid.Parse("da395715-fd95-43a5-a668-f8e8adb642db"),
                     Frequency = "15m",
                     DataDate = DateTime.Parse("2022-11-27 21:59:59"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "kMT",
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
                     ProductLinkId = Guid.Parse("da395715-fd95-43a5-a668-f8e8adb642db"),
                     Frequency = "quarterly",
                     DataDate = DateTime.Parse("2022-11-27"),
                     IsDaily = true,
                     IsMonthly = true,
                     IsMonthToDate = false,
                     IsRealTime = false,
                     UomName = "kMT",
                     Color ="red",
                     NorCode = "4",
                     Value = 34,
                     Variance = 34,
                     Percentage = 50,
                     NumValues = "{\r\n\"actual\":87.2061746636363636,\r\n\"variance_percentage\":75,\r\n\"planned\":75,\r\n\"variance\":11.20978,\r\n\"_variance\":11.20978\r\n}"
                 },
             };
            context.ProductLinkDetails?.AddRange(productLinkDetails);

            var uom = new List<UnitOfMeasure>
             {
                 new UnitOfMeasure
                 {
                     Name = "kMT",
                     Id = Guid.Parse("25c23226-2173-4235-9d3b-ffca9eeea649"),
                 },
             };
            context.UnitOfMeasures?.AddRange(uom);
            context.SaveChanges();
        }
        [Fact]
        public async Task GetChartData_Success()
        {
            var request = new ChartRequest
            {
                ProductLinkId = new List<Guid>
                 {
                     Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba")
                 },
                FromDate = DateTime.Parse("2022-07-31 17:00:00"),
                ToDate = DateTime.Parse("2022-11-27 16:59:59"),
                IsRealTime = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsWeekly = false,
                IsDaily = false,
                IsMonthly = true,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var result = await _mockChartService.GetChartData(request, default);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetChartDataDaily_Success()
        {
            var request = new ChartRequest
            {
                ProductLinkId = new List<Guid>
                 {
                     Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba")
                 },
                FromDate = DateTime.Parse("2022-07-31 17:00:00"),
                ToDate = DateTime.Parse("2022-11-27 16:59:59"),
                IsRealTime = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsWeekly = false,
                IsDaily = true,
                IsMonthly = false,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var result = await _mockChartService.GetChartData(request, default);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetChartData15m_Success()
        {
            var request = new ChartRequest
            {
                ProductLinkId = new List<Guid>
                 {
                     Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba")
                 },
                FromDate = DateTime.Parse("2022-07-31 17:00:00"),
                ToDate = DateTime.Parse("2022-11-27 16:59:59"),
                IsRealTime = true,
                IsHour = false,
                IsQuarterToDate = false,
                IsWeekly = false,
                IsDaily = false,
                IsMonthly = false,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var result = await _mockChartService.GetChartData(request, default);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetChartDataHourly_Success()
        {
            var request = new ChartRequest
            {
                ProductLinkId = new List<Guid>
                 {
                     Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba")
                 },
                FromDate = DateTime.Parse("2022-07-31 17:00:00"),
                ToDate = DateTime.Parse("2022-11-27 16:59:59"),
                IsRealTime = false,
                IsHour = true,
                IsQuarterToDate = false,
                IsWeekly = false,
                IsDaily = false,
                IsMonthly = false,
                IsMonthToDate = false,
                IsQuarterly = false,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var result = await _mockChartService.GetChartData(request, default);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetChartDataQuarterly_Success()
        {
            var request = new ChartRequest
            {
                ProductLinkId = new List<Guid>
                 {
                     Guid.Parse("71e897a2-c6ca-8241-740c-0c4a9fb201ba")
                 },
                FromDate = DateTime.Parse("2022-07-31 17:00:00"),
                ToDate = DateTime.Parse("2022-11-27 16:59:59"),
                IsRealTime = false,
                IsHour = false,
                IsQuarterToDate = false,
                IsWeekly = false,
                IsDaily = false,
                IsMonthly = false,
                IsMonthToDate = false,
                IsQuarterly = true,
                IsYearToDaily = false,
                IsYearToMonthly = false,
                IsYearEndProjection = false
            };
            var result = await _mockChartService.GetChartData(request, default);
            Assert.NotNull(result);
        }

    }
}