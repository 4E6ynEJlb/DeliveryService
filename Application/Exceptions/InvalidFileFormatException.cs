using Domain.Models.ApplicationModels;
using Domain.Models.ApplicationModels.Exceptions;
using System.Net;

namespace Application.Exceptions
{

    public class InvalidFileFormatException : ClientException
    {
        public InvalidFileFormatException()
        {
            Message = ExceptionsMessages.INVALID_FILE_FORMAT_EXCEPTION_MESSAGE;
            Code = HttpStatusCode.UnsupportedMediaType;
        }
    }
}
