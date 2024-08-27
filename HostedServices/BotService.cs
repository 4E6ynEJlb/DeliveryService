using Domain.Stores;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostedServices
{
    public class BotService : IHostedService
    {
        ITelegramBotApi BotApi;
        IUserStore UserStore;
        public BotService(ITelegramBotApi botApi, IServiceScopeFactory scopeFactory)
        {
            BotApi = botApi;
            UserStore = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserStore>();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            BotApi.Start(cancellationToken);
            var st = Environment.StackTrace;
            Exception ex = new Exception();
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            CongratulationTask(cancellationToken);
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await BotApi.StopAsync();
        }
        private async Task CongratulationTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
                List<string> ids = await UserStore.GetBirthdayPeopleTelegramAsync(cancellationToken);
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
                BotApi.SendCongratulationsToUsers(ids, cancellationToken);
#pragma warning restore CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
            }
        }
    }
}
