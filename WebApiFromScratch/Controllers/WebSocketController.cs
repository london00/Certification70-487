using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Web.WebSockets;

namespace WebApiFromScratch.Controllers
{
    public class WebSocketController : ApiController
    {
        private static List<WebSocket> WebSockets = new List<WebSocket>();

        [HttpGet]
        public HttpResponseMessage Connect()
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext.IsWebSocketRequest || currentContext.IsWebSocketRequestUpgrading)
            {
                currentContext.AcceptWebSocketRequest(Proccess);
                return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
            }

            return null;
        }

        public async Task Proccess(AspNetWebSocketContext context)
        {
            WebSockets.Add(context.WebSocket);

            while (true)
            {
                ArraySegment<byte> arraySegment = new ArraySegment<byte>(new byte[1024]);

                // open the result. This is waiting asynchronously
                WebSocketReceiveResult socketResult = await context.WebSocket.ReceiveAsync(arraySegment, CancellationToken.None);

                // return the message to the client if the socket is still open
                if (context.WebSocket.State == WebSocketState.Open)
                {
                    string message = Encoding.UTF8.GetString(arraySegment.Array, 0, socketResult.Count);

                    var userMessage = message + " - " + DateTime.Now.ToString();
                    arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMessage));

                    for (int i = 0; i < WebSockets.Count; i++)
                    {
                        try
                        {
                            // Asynchronously send a message to the client
                            await WebSockets.ElementAt(i).SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        catch (Exception ex)
                        {
                            // Remove corrupted connections
                            WebSockets.RemoveAt(i);
                            i--; // Return to previous index.
                        }
                    }

                    if (message.EndsWith(":Has leaved the room"))
                    {
                        // Remove from the pool conection
                        WebSockets.Remove(context.WebSocket);

                        CancellationToken cancelationToken = new CancellationToken();
                        await context.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "User has leaved the room", cancelationToken);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}