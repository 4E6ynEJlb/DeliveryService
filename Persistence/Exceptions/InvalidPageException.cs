using Domain.Models.ApplicationModels;
using Domain.Models.ApplicationModels.Exceptions;
using System.Net;

namespace Persistence.Exceptions
{
    public class InvalidPageException : ClientException
    {
        public InvalidPageException()
        {
            Message = ExceptionsMessages.INVALID_PAGE_EXCEPTION_MESSAGE;
            Code = HttpStatusCode.BadRequest;
        }
    }
}
