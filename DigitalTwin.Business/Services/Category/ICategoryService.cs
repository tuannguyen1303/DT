using DigitalTwin.Models.Requests.Category;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Business.Services.Category
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryResponse>>> GetCategories(GetCategoryRequest request, CancellationToken cancellationToken);
    }
}
