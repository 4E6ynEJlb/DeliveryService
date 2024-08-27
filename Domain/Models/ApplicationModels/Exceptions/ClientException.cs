using System.Net;

namespace Domain.Models.ApplicationModels.Exceptions
{
    /// <summary>
    /// DO NOT THROW this without using initializer for both of fields
    /// </summary>
    public class ClientException : Exception
    {
        public HttpStatusCode Code;
        public new string? Message;
    }
}
