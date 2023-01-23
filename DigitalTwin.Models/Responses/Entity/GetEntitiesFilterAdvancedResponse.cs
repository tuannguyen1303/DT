using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Responses.Entity
{
    public class GetEntitiesFilterAdvancedResponse
    {
        public List<EntitiesByTypeResponse>? EntitiesByTypeResponses { get; set; }
    }
}
