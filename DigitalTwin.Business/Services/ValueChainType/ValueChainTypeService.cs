using DigitalTwin.Data.Database;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Microsoft.EntityFrameworkCore;

namespace DigitalTwin.Business.Services.ValueChainType
{
    public class ValueChainTypeService : IValueChainTypeService
    {
        private readonly DigitalTwinContext _context;

        public ValueChainTypeService(DigitalTwinContext context)
        {
            _context = context;
        }

        public async Task<Response<List<ValueChainTypeResponse>>> GetAll(CancellationToken token)
        {
            var listValueChainType = new List<ValueChainTypeResponse>();

            listValueChainType = await _context.ValueChainTypes!.Select(_ => new ValueChainTypeResponse
            {
                Id = _.Id,
                Name = _.Name,
                Label = _.Name!.Split(new[] { ' ' }).First()
            }).ToListAsync(token);

            return Response.CreateResponse(listValueChainType);
        }
    }
}
