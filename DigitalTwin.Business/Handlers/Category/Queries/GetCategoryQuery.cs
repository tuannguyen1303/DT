using DigitalTwin.Business.Services.Category;
using DigitalTwin.Models.Requests.Category;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;
using MediatR;

namespace DigitalTwin.Business.Handlers.Category.Queries
{
    public class GetCategoryQuery : IRequestHandler<GetCategoryRequest, Response<List<CategoryResponse>>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryQuery(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<Response<List<CategoryResponse>>> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategories(request, cancellationToken);
        }
    }
}
