
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AMIntermediaCore;
using Newtonsoft.Json;

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

        public void SendAxeUpdate(Axe axe)
        {
            SendMessageToAllAsync("axe-update:" + JsonConvert.SerializeObject(axe));
        }

        public void SendAggregationUpdate(Aggregation axe)
        {
            SendMessageToAllAsync("aggregation-update:" + JsonConvert.SerializeObject(axe));
        }

        public void SendOrderUpdate(Order order)
        {
            SendMessageToAllAsync("order-update:" + JsonConvert.SerializeObject(order));
        }
    }
}