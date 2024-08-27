using Domain.Models.Entities.SQLEntities;

namespace Domain.Models.VievModels
{
    public class UserRegisterModel
    {
        public required string Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? TelegramId { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public User ToUser()
        {
            return new User { Id =  Guid.NewGuid(), Name = Name, BirthDate = BirthDate, TelegramId = TelegramId, Login = Login, Password = Password, IsAdmin = false, Bonuses = 0 };
        }
    }
}
