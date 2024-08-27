namespace Domain.Models.ApplicationModels
{
    public class ServicesOptions
    {
        public const string OptionsName = "ServicesOptions";
        public required string GoodsImagesLinkTemplate { get; set; }
        public required string GoodsImagesPath { get; set; }
        public required decimal BonusesForTelegram { get; set; }
        public required decimal BonusesForBirthdate { get; set; }
    }
}