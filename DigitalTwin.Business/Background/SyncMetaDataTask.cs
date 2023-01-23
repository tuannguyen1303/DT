using System.Data;
using DigitalTwin.Common.Constants;
using DigitalTwin.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DigitalTwin.Business.Background
{
    public class SyncMetaDataTask : ScheduledProcessor
    {
        private readonly ILogger<SyncMetaDataTask> _logger;

        public SyncMetaDataTask(IServiceScopeFactory serviceScopeFactory,
            ILogger<SyncMetaDataTask> logger) : base(serviceScopeFactory)
        {
            _logger = logger;
        }

        protected override string Schedule => ScheduleTimer.FiveMin;

        protected override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            _logger.LogInformation($"{nameof(SyncMetaDataTask)}'s ProcessInScope is running");

            try
            {
                using var scope = ServiceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DigitalTwinContext>();
                await using var command = context.Database.GetDbConnection().CreateCommand();
                
                command.CommandText = $"call pivot_da_middleware_digital.p_syncmasterdata({false})";
                command.CommandType = CommandType.Text;
                if (command.Connection is { State: ConnectionState.Closed })
                    await command.Connection.OpenAsync(StoppingCts.Token);

                var output = await command.ExecuteScalarAsync(StoppingCts.Token);
                var result = (bool)(output ?? false);
                
                await context.Database.CloseConnectionAsync();
                _logger.LogInformation(!result
                    ? $"{nameof(SyncMetaDataTask)} cannot sync the data"
                    : $"{nameof(SyncMetaDataTask)} can sync the data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SyncMetaDataTask)}'s ProcessInScope has error: {ex.Message}");
            }
            finally
            {
                _logger.LogInformation($"{nameof(SyncMetaDataTask)}'s ProcessInScope has finished");
            }
        }
    }
}