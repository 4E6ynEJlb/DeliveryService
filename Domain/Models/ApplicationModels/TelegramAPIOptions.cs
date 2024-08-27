namespace Domain.Models.ApplicationModels
{
    public class TelegramAPIOptions
    {
        public const string OptionsName = "TelegramOptions";
        public required string TgBotToken { get; set; }
        public long HostTgId { get; set; }
        public int HostAuthAccesMinutes { get; set; }
    }
}
