using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiFromScratch
{
    internal class LoggingMessageHandler : DelegatingHandler
    {
        public LoggingMessageHandler()
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("LoggingMessageHandler: Request started");
            var response = base.SendAsync(request, cancellationToken);
            Debug.WriteLine("LoggingMessageHandler: Request completed");

            return response;
        }
    }
}