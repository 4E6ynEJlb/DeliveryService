using System.Net;

namespace Domain.Models.ApplicationModels.Exceptions
{
    public class InvalidCardNumberException : ClientException
    {
        public InvalidCardNumberException()
        {
            Message = ExceptionsMessages.INVALID_CARD_NUMBER_EXCEPTION_MESSAGE;
            Code = HttpStatusCode.BadRequest;
        }
    }
}
