using System;
using Ckmio;

namespace AMIntermediaCore
{
    public class AggregationService
    {
        public string AxesStreamName {get; private set;}
        public string OrdersStreamName {get; private set;}
        public CkmioClient MioClient {get; set;}

        public AggregationService(string axesStreamName, string ordersStreamName)
        {
            this.AxesStreamName = axesStreamName;
            this.OrdersStreamName = ordersStreamName;
        }
        public void Start()
        {
            if(MioClient == null){
                MioClient = new CkmioClient( "community-test-key",
                                "community-test-secret", 
                                "Producer", "xpassw0rd");

                MioClient.Start();
            }
            MioClient.StartFunnel(AxesStreamName, new FunnelCondition[]{});
            MioClient.StartFunnel(OrdersStreamName, new FunnelCondition[]{});
            MioClient.FunnelUpdateHandler = OnNewAxe;
        }

        public void OnNewAxe(FunnelUpdate axeUpdate)
        {
            if(axeUpdate.Stream == AxesStreamName)
                Console.WriteLine($"Received a new axe :\n {axeUpdate.Content}");

            if(axeUpdate.Stream == OrdersStreamName)
                Console.WriteLine($"Received a new Order :\n {axeUpdate.Content}");
        }
    }
}