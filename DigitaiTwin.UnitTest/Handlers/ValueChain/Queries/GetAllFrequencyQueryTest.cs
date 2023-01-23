using DigitalTwin.Business.Handlers.ValueChain.Queries;
using DigitalTwin.Business.Services.ValueChain;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using Moq;

namespace DigitaiTwin.UnitTest.Handlers.ValueChain.Queries;

public class GetAllFrequencyQueryTest
{
    private readonly Mock<IValueChainService> _mockService;

    public GetAllFrequencyQueryTest()
    {
        _mockService = new Mock<IValueChainService>();
    }

    [Fact]
    public async Task GetAllFrequency_HasData()
    {
        var returnedFrequencies = new GetAllFrequencyResponse
        {
            Frequencies = new List<string?>
             {
                 "daily",
                 "monthly"
             }
        };

        var response = Response.CreateResponse(returnedFrequencies);
        _mockService.Setup(p => p.GetFrequencies(default)).ReturnsAsync(response);

        var query = new GetAllFrequencyQuery(_mockService.Object);
        var result = await query.Handle(new GetAllFrequencyRequest(), default);
        Assert.True(result?.Result != null);
    }
}