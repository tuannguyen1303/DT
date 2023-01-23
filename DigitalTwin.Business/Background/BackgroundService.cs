using Microsoft.Extensions.Hosting;

namespace DigitalTwin.Business.Background
{
    public abstract class BackgroundService : IHostedService
    {
        private Task? _executingTask;
        protected readonly CancellationTokenSource StoppingCts = new();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(StoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            // Otherwise it's running
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                StoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Process();

                await Task.Delay(5000, stoppingToken); //5 seconds delay
            } while (!stoppingToken.IsCancellationRequested);
        }

        protected abstract Task Process();
    }
}