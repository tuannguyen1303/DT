using DigitalTwin.Models.Responses.ValueChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Responses.Entity
{
    public class EntitiesByTypeResponse
    {
        public string? EntityGroupName { get; set; }
        public int Count { get; set; }
        public List<EntityResponse>? Entities { get; set; }
    }
}
