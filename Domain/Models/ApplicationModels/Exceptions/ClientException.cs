using System.Net;

namespace Domain.Models.ApplicationModels.Exceptions
{
    public class ClientException : Exception
    {
        public HttpStatusCode Code;
        public new string? Message;
    }
}
