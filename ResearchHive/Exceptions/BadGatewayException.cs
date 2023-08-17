using System.Net;

namespace Application.Common.Exceptions
{
    public class BadGatewayException : CustomException
    {
        public BadGatewayException(string message)
        : base(message, null, HttpStatusCode.BadGateway)
        {
        }
    }
}
