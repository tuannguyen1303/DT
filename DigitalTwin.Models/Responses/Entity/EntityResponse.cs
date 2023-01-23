using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Responses.Entity
{
    public class EntityResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid EntityId { get; set; }
        public string? Alias { get; set; }
        public string? Type { get; set; }
        public string? Path { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
    }
}
