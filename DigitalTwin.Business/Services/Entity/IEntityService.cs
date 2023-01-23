using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;
using DigitalTwin.Models.Responses.ValueChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Business.Services.Entity
{
    public interface IEntityService
    {
        Task<Response<GetEntitiesFilterAdvancedResponse>> GetAllEntities(GetAllEntitiesRequest request, CancellationToken cancellationToken);
    }
}
