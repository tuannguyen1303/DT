using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Common.Constants;
using DigitalTwin.Models.Requests.Category;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;

namespace DigitalTwin.Business.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IValueChainService _valueChainService;

        public CategoryService(IValueChainService valueChainService)
        {
            _valueChainService = valueChainService;
        }

        public async Task<Response<List<CategoryResponse>>> GetCategories(GetCategoryRequest request,
            CancellationToken cancellationToken)
        {
            var valueChainRequest = new ValueChainRequest
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                ValueChainTypeId = request.ValueChainTypeId,
                IsDaily = request.IsDaily,
                IsMonthly = request.IsMonthly,
                IsMonthToDate = request.IsMonthToDate,
                IsQuarterly = request.IsQuarterly,
                IsYearEndProjection = request.IsYearEndProjection,
                IsYearToDaily = request.IsYearToDaily,
                IsYearToMonthly = request.IsYearToMonthly,
                IsRealTime = request.IsRealTime,
                IsHour = request.IsHour,
                IsQuarterToDate = request.IsQuarterToDate,
                IsWeekly = request.IsWeekly,
                IsFilter = false,
            };
            var categories = request.Category!.Split("/");
            var entityResponse = await _valueChainService.GetValueChains(valueChainRequest, cancellationToken);

            var categoryResponses = new List<CategoryResponse>();
            if (categories[0] == DigitalTwinConstants.BusinessCateogry)
            {
                var entityDtos = entityResponse.Entities?.Where(c =>
                    DigitalTwinConstants.BusinessTab.Contains(c.Category!) &&
                    (!c.ParentList!.Any() || (c.ParentList!.All(f => !DigitalTwinConstants.BusinessTab.Contains(f.Category!))))).ToList();

                var businessCategory = new List<CategoryResponse>();
                foreach (var entityDto in entityDtos!)
                {
                    var categoryChildren = new List<CategoryInfomation>();
                    categoryChildren.AddRange(entityResponse.Products!.Where(d => d.Parent == entityDto.EntityId)
                        .Select(
                            d => new CategoryInfomation
                            {
                                Id = d.Id,
                                Name = entityDto.Name,
                                Actual = d.Value != null ? d.Value!.Value.ToString("N2") : "0.00",
                                Planned = d.Planned != null
                                    ? d.Planned!.Value.ToString("N2")
                                    : "0.00",
                                Variance = d.Variance != null ? d.Variance!.Value.ToString("N2") : "0.00",
                                Unit = d.Unit,
                            }));

                    var category = categoryChildren.GroupBy(c => c.Id).Select(d => d.First()).GroupBy(d => d.Unit)
                        .Select(
                            c => new CategoryResponse
                            {
                                Name = entityDto.Name,
                                Id = entityDto!.Id,
                                Actual = c.Sum(c => decimal.Parse(c.Actual!)).ToString("N2"),
                                Planned = c.Sum(c => decimal.Parse(c.Planned!)).ToString("N2"),
                                Variance = c.Sum(c => decimal.Parse(c.Variance!)).ToString("N2"),
                                Unit = c.Key,
                                ChildCategories = c.ToList()
                            });

                    businessCategory.AddRange(category);
                }

                categoryResponses = businessCategory.GroupBy(c => c.Unit).Select(c => new CategoryResponse
                {
                    Name = DigitalTwinConstants.BusinessType,
                    Id = c.FirstOrDefault()!.Id,
                    Actual = c.Sum(c => decimal.Parse(c.Actual!)).ToString("N2"),
                    Planned = c.Sum(c => decimal.Parse(c.Planned!)).ToString("N2"),
                    Variance = c.Sum(c => decimal.Parse(c.Variance!)).ToString("N2"),
                    Unit = c.Key,
                    ChildCategories = c.Select(d => new CategoryInfomation
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Actual = !string.IsNullOrEmpty(d.Actual)
                            ? decimal.Parse(d.Actual!).ToString("N2")
                            : "0.00",
                        Planned = !string.IsNullOrEmpty(d.Planned)
                            ? decimal.Parse(d.Planned!).ToString("N2")
                            : "0.00",
                        Variance = !string.IsNullOrEmpty(d.Variance)
                            ? decimal.Parse(d.Variance!).ToString("N2")
                            : "0.00",
                        Unit = d.Unit,
                    }).ToList(),
                }).ToList();
            }
            else if (categories[0] == DigitalTwinConstants.ProductCateogry)
            {
                var category = entityResponse.Products!.GroupBy(c => new { c.Name, c.Unit }).Select(
                    c => new CategoryResponse
                    {
                        Name = c.Key.Name,
                        Id = c.First().Id,
                        Actual = c.Sum(d => d.Value!)!.Value.ToString("N2"),
                        Planned = c.Sum(d => d.Planned != null ? d.Planned! : 0)!.Value.ToString("N2"),
                        Variance = c.Sum(d => d.Variance!)!.Value.ToString("N2"),
                        Unit = c.Key.Unit,
                        ChildCategories = c.Select(d => new CategoryInfomation
                        {
                            Id = d.Id,
                            Name = d.Alias,
                            Actual = d.Value != null ? d.Value!.Value.ToString("N2") : "0.00",
                            Planned = d.Planned != null ? d.Planned!.Value.ToString("N2") : "0.00",
                            Variance = d.Variance != null ? d.Variance!.Value.ToString("N2") : "0.00",
                            Unit = d.Unit
                        }).OrderBy(c => c.Name).ToList()
                    });

                categoryResponses.AddRange(category);
            }
            else if (DigitalTwinConstants.Deliveries.Contains(categories[0]))
            {
                var entityDtos = entityResponse.Entities?.Where(c => categories.Contains(c.Category)).ToList();
                foreach (var entityDto in entityDtos!)
                {
                    var categoryChildrent = new List<CategoryInfomation>();

                    categoryChildrent.AddRange(entityResponse.Products!.Where(d => d.Children == entityDto.EntityId)
                        .Select(
                            d => new CategoryInfomation
                            {
                                Id = d.Id,
                                Name = entityDto.Name,
                                Actual = d.Value != null ? d.Value!.Value.ToString("N2") : "0.00",
                                Planned = d.Planned != null ? d.Planned!.Value.ToString("N2") : "0.00",
                                Variance = d.Variance != null ? d.Variance!.Value.ToString("N2") : "0.00",
                                Unit = d.Unit
                            }));

                    var category = categoryChildrent.GroupBy(c => c.Id).Select(d => d.First()).GroupBy(d => d.Unit)
                        .Select(
                            c => new CategoryResponse
                            {
                                Name = entityDto.Name,
                                Id = entityDto!.Id,
                                Actual = c.Sum(c => decimal.Parse(c.Actual!)).ToString("N2"),
                                Planned = c.Sum(c => decimal.Parse(c.Planned!)).ToString("N2"),
                                Variance = c.Sum(c => decimal.Parse(c.Variance!)).ToString("N2"),
                                Unit = c.Key,
                                ChildCategories = DigitalTwinConstants.Deliveries.Contains(categories[0])
                                    ? null
                                    : c.ToList()
                            });

                    categoryResponses.AddRange(category);
                }
            }
            else
            {
                var entityDtos = entityResponse.Entities?.Where(c => categories.Contains(c.Category)).ToList();
                foreach (var entityDto in entityDtos!)
                {
                    var categoryChildrent = new List<CategoryInfomation>();

                    categoryChildrent.AddRange(entityResponse.Products!.Where(d => d.Parent == entityDto.EntityId)
                        .Select(
                            d => new CategoryInfomation
                            {
                                Id = d.Id,
                                Name = d.Alias!.Split(" -> ")[1],
                                Actual = d.Value != null ? d.Value!.Value.ToString("N2") : "0.00",
                                Planned = d.Planned != null ? d.Planned!.Value.ToString("N2") : "0.00",
                                Variance = d.Variance != null ? d.Variance!.Value.ToString("N2") : "0.00",
                                Unit = d.Unit
                            }));

                    var category = categoryChildrent.GroupBy(c => c.Id).Select(d => d.First()).GroupBy(d => d.Unit)
                        .Select(
                            c => new CategoryResponse
                            {
                                Name = entityDto.Name,
                                Id = entityDto!.Id,
                                Actual = c.Sum(c => decimal.Parse(c.Actual!)).ToString("N2"),
                                Planned = c.Sum(c => decimal.Parse(c.Planned!)).ToString("N2"),
                                Variance = c.Sum(c => decimal.Parse(c.Variance!)).ToString("N2"),
                                Unit = c.Key,
                                ChildCategories = DigitalTwinConstants.Deliveries.Contains(categories[0])
                                    ? null
                                    : c.ToList()
                            });

                    categoryResponses.AddRange(category);
                }
            }

            return await Task.FromResult(Response.CreateResponse(categoryResponses.OrderBy(c => c.Name).ToList()));
        }
    }
}