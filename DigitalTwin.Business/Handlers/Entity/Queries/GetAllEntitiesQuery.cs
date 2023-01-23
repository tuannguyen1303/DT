using DigitalTwin.Business.Services.Entity;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;
using MediatR;

namespace DigitalTwin.Business.Handlers.Entity.Queries
{
    public class GetAllEntitiesQuery : IRequestHandler<GetAllEntitiesRequest, Response<GetEntitiesFilterAdvancedResponse>>
    {
        private readonly IEntityService _entityService;

        public GetAllEntitiesQuery(IEntityService entityService)
        {
            _entityService = entityService;
        }

        public async Task<Response<GetEntitiesFilterAdvancedResponse>> Handle(GetAllEntitiesRequest request, CancellationToken cancellationToken)
        {
            return await _entityService.GetAllEntities(request, cancellationToken);
        }
    }
}
