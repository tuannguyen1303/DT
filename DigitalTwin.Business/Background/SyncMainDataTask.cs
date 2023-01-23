using System.Data;
using DigitalTwin.Common.Constants;
using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigitalTwin.Business.Background;

public class SyncMainDataTask : ScheduledProcessor
{
    private readonly ILogger<SyncMainDataTask> _logger;

    public SyncMainDataTask(IServiceScopeFactory serviceScopeFactory, ILogger<SyncMainDataTask> logger) : base(
        serviceScopeFactory)
    {
        _logger = logger;
    }

    protected override async Task ProcessInScope(IServiceProvider serviceProvider)
    {
        _logger.LogInformation($"{nameof(SyncMainDataTask)}'s ProcessInScope is running");

        try
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DigitalTwinContext>();
            await using var command = context.Database.GetDbConnection().CreateCommand();

            command.CommandText = $"call pivot_da_middleware_digital.p_syncentities({false})";
            command.CommandType = CommandType.Text;
            if (command.Connection is { State: ConnectionState.Closed })
                await command.Connection.OpenAsync(StoppingCts.Token);

            var output = await command.ExecuteScalarAsync(StoppingCts.Token);
            var result = (bool)(output ?? false);

            await context.Database.CloseConnectionAsync();
            _logger.LogInformation(!result
                ? $"{nameof(SyncMainDataTask)} cannot sync the data"
                : $"{nameof(SyncMainDataTask)} can sync the data");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(SyncMainDataTask)}'s ProcessInScope has error: {ex.Message}");
        }
        finally
        {
            _logger.LogInformation($"{nameof(SyncMainDataTask)}'s ProcessInScope has finished");
        }
    }

    protected override string Schedule => ScheduleTimer.FifteenMin;
}