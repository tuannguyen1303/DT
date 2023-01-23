using DigitalTwin.Business.Handlers.Category.Queries;
using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.Category;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.Category;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Handlers.Category.Queries
{
    public class GetCategoryQueryTest
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        public GetCategoryQueryTest()
        {
            _mockCategoryService = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task GetProduct_Success_HasData()
        {
            var request = new GetCategoryRequest
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
                IsHour = false,
                IsRealTime = false,
                PageSize = 10,
                PageNumber = 0,
                Category = "Business",
            };

            var query = new GetCategoryQuery(_mockCategoryService.Object);

            var categoryData = new List<CategoryResponse>
             {
                    new CategoryResponse
                    {
                        Name = "MRC",
                        Actual = "-34",
                        Planned = "433",
                        Variance = "-46",
                        Id = Guid.NewGuid(),
                        ChildCategories = new List<CategoryInfomation>()
                        {
                            new CategoryInfomation
                            {
                                Name = "MRC",
                                Actual = "-34",
                                Planned = "433",
                                Variance = "-46",
                                Id = Guid.NewGuid()
                            }
                        }
                     },
                    new CategoryResponse
                    {

                        Name = "F&M",
                        Actual = "-34",
                        Planned = "433",
                        Variance = "-46",
                        Id = Guid.NewGuid(),
                        ChildCategories = new List<CategoryInfomation>()
                        {
                            new CategoryInfomation
                            {
                                Name = "F&M",
                                Actual = "-34",
                                Planned = "433",
                                Variance = "-46",
                                Id = Guid.NewGuid()
                            }
                        }
                    }
             };

            var expected = Response.CreateResponse(categoryData);

            _mockCategoryService.Setup(p => p.GetCategories(It.IsAny<GetCategoryRequest>(), default)).ReturnsAsync(expected);
            var result = await query.Handle(request, default);

            Assert.Equal(expected, result);
        }
    }
}
