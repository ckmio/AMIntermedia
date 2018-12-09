
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace AMIntermediaWeb
{
    public class DashboardRealtimeHandler : WebSocketHandler 
    {
        public DashboardRealtimeHandler(WebSocketManager manager):base(manager){    } 

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string receivedStr = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"received string \n {receivedStr}");
            return Task.CompletedTask;
        }
    }
}