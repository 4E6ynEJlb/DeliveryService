using Domain.Models.ApplicationModels;
using Domain.Models.ApplicationModels.Exceptions;
using System.Net;

namespace Application.Exceptions
{
    public class InvalidMarkException : ClientException
    {
        public InvalidMarkException()
        {
            Message = ExceptionsMessages.INVALID_MARK_EXCEPTION_MESSAGE;
            Code = HttpStatusCode.BadRequest;
        }
    }
}
