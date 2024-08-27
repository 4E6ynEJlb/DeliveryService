using System.Net;

namespace Domain.Models.ApplicationModels.Exceptions
{
    public abstract class ClientException : Exception
    {
        public HttpStatusCode Code;
        public new string? Message;
    }
}
