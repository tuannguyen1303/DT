using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Responses.ValueChain
{
    public class GetProductDetailResponse
    {
        public ChartData? ChartData { get; set; }
        public DataValueChainDetail? Data { get; set; }
        public List<ProductionVolume>? ProductionVolume { get; set; }
        public bool IsShowChart { get; set; }
        public bool IsNonFrequency { get; set; }
        public bool IsTarget { get; set; }
    }
}
