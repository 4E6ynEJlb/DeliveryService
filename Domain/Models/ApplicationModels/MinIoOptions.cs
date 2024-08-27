namespace Domain.Models.ApplicationModels
{
    public class MinIoOptions
    {
        public const string OptionsName = "MinIoOptions";
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
        public required string Endpoint { get; set; }
        public required string Bucket { get; set; }
    }
}
