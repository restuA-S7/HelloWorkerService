using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceDemo
{
    internal class MyWorker : BackgroundService
    {

        private readonly ILogger<MyWorker> _logger; //variabel

        public MyWorker(ILogger<MyWorker> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("MyWorker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
