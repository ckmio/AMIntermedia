using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Ckmio;

namespace AMIntermediaCore
{
    public class AggregationService
    {
        public string AxesStreamName {get; private set;}
        public string OrdersStreamName {get; private set;}
        public CkmioClient MioClient {get; set;}

        public List<Axe> Axes {get; private set;}
        public List<Order> Orders {get; private set;}

        public Action<Axe> AxesUpdateHandler {get; set;}

        public Action<Order> OrdersAdditionHandler {get; set;}

        public Action<Order> OrdersUpdateHandler {get; set;}

        public AggregationService()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
            .Build();
            this.AxesStreamName = config["AMIntermediaServices:AxesStreamName"];;;
            this.OrdersStreamName = config["AMIntermediaServices:OrdersStreamName"];
            this.Axes = new List<Axe>();
            this.Orders = new List<Order>();
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
                var axeDict =(JObject)((JObject)axeUpdate.Content)["content"];
                var axe = Axe.FromJObject(axeDict);
                var affectedOrders = SaveAxeAndReturnUpdatedOrders(axe);
                if(AxesUpdateHandler!=null) AxesUpdateHandler(axe);
                if(OrdersUpdateHandler!=null) affectedOrders.ForEach(o => OrdersUpdateHandler(o));
                return;
            }

            if(axeUpdate.Stream == OrdersStreamName && axeUpdate.Content != String.Empty)
            {
                var orderDict = (JObject)axeUpdate.Content;
                var order = Order.FromJObject(orderDict);
                order.Axes = FindLatestAxes(order.ISIN);
                Orders.Add(order);
                if(OrdersAdditionHandler!=null) OrdersAdditionHandler(order);
            }
        }

        public List<Order> SaveAxeAndReturnUpdatedOrders(Axe axe)
        {
            Axes.Add(axe);
            var affectedOrders =  Orders.Where(o => o.ISIN == axe.ISIN).ToList(); 
            affectedOrders.ForEach((o) => o.Axes.Add(axe));
            return affectedOrders;
        }

        public List<Axe> FindLatestAxes(string ISIN)
        {
            return Axes.Where(a => a.ISIN == ISIN).ToList();
        }
    }
}