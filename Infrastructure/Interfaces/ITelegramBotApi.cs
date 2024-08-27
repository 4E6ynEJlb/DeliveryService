namespace Infrastructure.Interfaces
{
    public interface ITelegramBotApi
    {
        public bool IsHostLogined { get; }
        public Task SendCongratulationsToUsers(List<string> idList, CancellationToken cancellationToken);
        public Task SendHostAuthMessageAsync(CancellationToken cancellationToken);
        public void Start(CancellationToken cancellationToken);
        public Task StopAsync();
    }
}
