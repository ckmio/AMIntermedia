using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ckmio;

namespace AMIntermediaCore
{
    public class AggregationService
    {
        public string AxesStreamName {get; private set;}
        public string OrdersStreamName {get; private set;}
        public CkmioClient MioClient {get; set;}

        public List<Axe> Axes {get; private set;}

        public AggregationService(string axesStreamName, string ordersStreamName)
        {
            this.AxesStreamName = axesStreamName;
            this.OrdersStreamName = ordersStreamName;
            this.Axes = new List<Axe>();
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
            if(axeUpdate.Stream == AxesStreamName && axeUpdate.Content != String.Empty)
            {
                SaveAxe((JObject)axeUpdate.Content);
                Console.WriteLine($"Received a new axe :\n {axeUpdate.Content}");
                return;
            }

            if(axeUpdate.Stream == OrdersStreamName && axeUpdate.Content != String.Empty)
            {
                var orderDict = (JObject)axeUpdate.Content ;
                Console.WriteLine($"Received a new Order :\n {orderDict["Desk"]}");
            }
        }

        public void SaveAxe(JObject content)
        {
            Console.WriteLine("ISIN_CODE : "+ ((JObject)content["content"])["ISIN_CODE"]);
        }

        public List<Axe> FindLatestAxes(string ISIN)
        {
            return Axes.Where(a => a.ISIN == ISIN).ToList();
        }
    }
}