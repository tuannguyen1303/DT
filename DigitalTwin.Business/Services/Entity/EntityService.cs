using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.Entity;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Entity;

namespace DigitalTwin.Business.Services.Entity
{
    public class EntityService : IEntityService
    {
        private readonly IValueChainService _valueChainService;

        public EntityService(IValueChainService valueChainService)
        {
            _valueChainService = valueChainService;
        }

        public async Task<Response<GetEntitiesFilterAdvancedResponse>> GetAllEntities(GetAllEntitiesRequest request,
            CancellationToken cancellationToken)
        {
            var valueChainRequest = new ValueChainRequest
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                ValueChainTypeId = request.ValueChainType,
                IsDaily = request.IsDaily,
                IsMonthly = request.IsMonthly,
                IsMonthToDate = request.IsMonthToDate,
                IsQuarterly = request.IsQuarterly,
                IsYearEndProjection = request.IsYearEndProjection,
                IsYearToDaily = request.IsYearToDaily,
                IsYearToMonthly = request.IsYearToMonthly,
                IsHour = request.IsHour,
                IsRealTime = request.IsRealTime,
                IsWeekly = request.IsWeekly,
                IsQuarterToDate = request.IsQuarterToDate,
                IsFilter = false,
            };

            var entityResponse = await _valueChainService.GetValueChains(valueChainRequest, cancellationToken);
            var entityFilters = new List<EntitiesByTypeResponse>();
            var groupName = entityResponse.Entities!.Select(c => c.Category).Distinct().ToList();
            foreach (var entityType in groupName)
            {
                var entitiesQuery = entityResponse.Entities!
                    .Where(e => e.Category!.ToLower() == entityType!.ToLower())
                    .Select(e => new EntityResponse
                    {
                        Id = e.Id,
                        EntityId = e.EntityId,
                        Name = e.Name,
                        Alias = e.Name!.ToLower(),
                        Category = e.Category,
                        Type = e.Type,
                    }).Distinct().ToList();

                var entityFilter = new EntitiesByTypeResponse
                {
                    EntityGroupName = entityType,
                    Entities = entitiesQuery,
                    Count = entitiesQuery.Count()
                };

                entityFilters.Add(entityFilter);
            }

            return await Task.FromResult(Response.CreateResponse(new GetEntitiesFilterAdvancedResponse
            {
                EntitiesByTypeResponses = entityFilters
            }));
        }
    }
}