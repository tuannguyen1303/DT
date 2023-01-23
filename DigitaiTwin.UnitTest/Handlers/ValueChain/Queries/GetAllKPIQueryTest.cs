using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitaiTwin.UnitTest.Handlers.ValueChain.Queries;

public class GetAllKPIQueryTest
{
    private readonly Mock<IValueChainService> _mockValueChainService;
    private readonly GetAllKPIQuery _handler;

    public GetAllKPIQueryTest()
    {
        _mockValueChainService = new Mock<IValueChainService>();
        _handler = new GetAllKPIQuery(_mockValueChainService.Object);
    }

    [Fact]
    public async Task GetAllKPIQuery_Success()
    {
        var response = new GetAllKPIResponse
        {
            ProductList = new List<ProductDto>
             {
                 new()
                 {
                     Id = Guid.Empty
                 }
             }
        };

        var expected = Response.CreateResponse(response);
        _mockValueChainService.Setup(p => p.GetAllKPI(It.IsAny<GetAllKPIRequest>(), default)).ReturnsAsync(expected);

        var result = await _handler.Handle(new GetAllKPIRequest(), default);
        Assert.NotNull(result.Result);
        Assert.Equal(result?.Result?.ProductList?.Count, expected?.Result?.ProductList?.Count);
    }
}