using System;
using System.Timers;
using Ckmio;

namespace AMIntermediaCore
{
    public class OrdersPullingService
    {
        public string OrdersStream {get; set;}
        public Random RandNumberProvider {get; set;}
        public Timer PullingTimer {get; set; }
        public int IntervalInMilliseconds {get; set;}
        public CkmioClient MioClient {get; private set;}

        public OrdersPullingService(string ordersStream, int pullingInterval)
        {
            this.OrdersStream = ordersStream;
            this.IntervalInMilliseconds = pullingInterval;
            this.PullingTimer = new System.Timers.Timer();
            this.PullingTimer.Interval =  pullingInterval;
            this.RandNumberProvider = new Random();
        }

        public void Start()
        {
            if(MioClient == null){
                MioClient = new CkmioClient( "community-test-key",
                                "community-test-secret", 
                                "Producer", "xpassw0rd");

                MioClient.Start();
            }
            /* Max number of entries between to update 100  */
            int maxUpdate = 100;
            PullingTimer.Elapsed += UpdateStream;
            PullingTimer.Enabled = true;
        }

        public void UpdateStream(object sender, ElapsedEventArgs eventArgs)
        {
            MioClient.SendToStream(OrdersStream, new {id = "Hello", name = "Name" });
        }

        public Order RandomOrder()
        {
            return null;
        }
    }
}