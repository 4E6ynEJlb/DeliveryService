namespace Domain.Models.ApplicationModels
{
    public class RepositoryOptions
    {
        public const string OptionsName = "RepositoryOptions";
        public int AuditExpirationDays { get; set; }
        public int HotGoodsExpirationHours { get; set; }
        public int CacheExpirationMins { get; set; }
    }
}
