using DigitalTwin.Models.Requests.Category;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<Response<List<CategoryResponse>>> GetCategories([FromBody] GetCategoryRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            return result;
        }
    }
}
