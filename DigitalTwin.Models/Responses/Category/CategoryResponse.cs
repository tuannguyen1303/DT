using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Responses.Category
{   
    public class CategoryResponse
    {
        public string? Name { get; set; }
        public string? Actual { get; set; }
        public string? Planned { get; set; }
        public string? Variance { get; set; }
        public string? Unit { get; set; }
        public Guid Id { get; set; }
        public List<CategoryInfomation>? ChildCategories { get; set; }
    }

    public class CategoryInfomation
    {
        public string? Name { get; set; }
        public string? Actual { get; set; }
        public string? Planned { get; set; }
        public string? Variance { get; set; }
        public string? Kpbi { get; set; }
        public Guid Id { get; set; }
        public string? Unit { get; set; }
        public string? Alias { get; set; }
        public string? Color { get; set; }
        public Guid? EntityId { get; set; }
    }
}
