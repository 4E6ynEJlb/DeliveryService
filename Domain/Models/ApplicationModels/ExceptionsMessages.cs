namespace Domain.Models.ApplicationModels
{
    public class ExceptionsMessages
    {
        public const string INVALID_CARD_NUMBER_EXCEPTION_MESSAGE = "Некорректный номер карты";
        public const string DOES_NOT_EXIST_EXCEPTION_MESSAGE = "не существует";
        public const string INVALID_PAGE_EXCEPTION_MESSAGE = "Номер страницы некорректный или не существует";
        public const string WAS_ALREADY_SET_EXCEPTION_MESSAGE = "нельзя установить повторно";
        public const string WAS_ALREADY_SET_EXCEPTION_UNKNOWN_PROPERTY_MESSAGE = "Неизвестное свойство";
        public const string INVALID_FILE_FORMAT_EXCEPTION_MESSAGE = "Отправленный файл не является изображением";
        public const string INVALID_MARK_EXCEPTION_MESSAGE = "Оценка не может быть меньше 0 или больше 5";
    }
}
