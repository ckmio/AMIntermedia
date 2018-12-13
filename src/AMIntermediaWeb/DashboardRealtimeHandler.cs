
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
        AggregationService AggregService {get; set;}
        public DashboardRealtimeHandler(WebSocketManager manager, AggregationService aggregrationService):base(manager){   
            this.AggregService = aggregrationService;
         }

        
        public override async Task OnConnected(WebSocket socket)
        {
            base.OnConnected(socket);
            SendMessageAsync(socket, String.Format("init:{0}",JsonConvert.SerializeObject(AggregService.Orders)));
        }

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
             Console.WriteLine($"order update for isin: {order.ISIN}\n");
             SendMessageToAllAsync("order-update:" + JsonConvert.SerializeObject(order));
        }

         public void OrdersAdditionHandler(Order order)
        {
            SendMessageToAllAsync("order-add:" + JsonConvert.SerializeObject(order));
        }
    }
}