using Domain.Models.ApplicationModels;
using Domain.Models.ApplicationModels.Exceptions;
using System.Net;

namespace Infrastructure
{
    public class DoesNotExistException : ClientException
    {
        public DoesNotExistException(Type entityType)
        {
            Message = $"{entityType.Name} {ExceptionsMessages.DOES_NOT_EXIST_EXCEPTION_MESSAGE}";
            Code = HttpStatusCode.NotFound;
        }
    }
}
