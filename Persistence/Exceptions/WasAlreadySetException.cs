using Domain.Models.ApplicationModels;
using Domain.Models.ApplicationModels.Exceptions;
using System.Net;

namespace Persistence.Exceptions
{
    public class WasAlreadySetException : ClientException
    {
        public WasAlreadySetException(string? name)
        {
            name ??= ExceptionsMessages.WAS_ALREADY_SET_EXCEPTION_UNKNOWN_PROPERTY_MESSAGE;
            Message = $"{name} {ExceptionsMessages.WAS_ALREADY_SET_EXCEPTION_MESSAGE}";
            Code = HttpStatusCode.Forbidden;
        }
    }
}
