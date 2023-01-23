using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalTwin.Api.Controllers
{
    public class EntityController : BaseController
    {
        public EntityController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<Response<GetEntitiesFilterAdvancedResponse>> GetEntitiesFilterAdvanced([FromBody] GetAllEntitiesRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            return result;
        }
    }
}
