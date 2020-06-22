using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Acme.Features.Orders
{
    public class Restocker : IHostedService
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly ProductCatalog _catalog;

        public Restocker(ProductCatalog catalog)
        {
            _catalog = catalog;            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

            if(_executingTask == null) return;

            try
            {
                _stoppingCts.Cancel();                
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

        }

        private async Task ExecuteAsync(CancellationToken token)
        {
            do
            {
                await Task.Delay(TimeSpan.FromMinutes(2), token);
                foreach(var product in _catalog.GetProducts())
                {
                    _catalog.Restock(product, 1000);
                }
            }
            while(!token.IsCancellationRequested);
        }
    }
}
