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

        public string[] Desks = new string[]{ "CREDIT","GOVIES","EMERGING" };
         public string[] ISINS = new string[]{ "FR0011962398","FR0125218232","NL0000102317","SK4120011420", "XS0460357550" };
         public string[] Portfolios = new string[]{ "FR0011962398","FR0125218232","NL0000102317","SK4120011420", "XS0460357550" };

        public static int TradeIndex {get; private set;}

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
            PullingTimer.Elapsed += UpdateStream;
            PullingTimer.Enabled = true;
        }

        public void UpdateStream(object sender, ElapsedEventArgs eventArgs)
        {
            MioClient.SendToStream(OrdersStream, RandomOrder());
        }

        public Order RandomOrder()
        {
            return new Order {
                Id = Guid.NewGuid().ToString(),
                TradeId = NextTradeId(),
                ISIN = RandomIsin(),
                Desk = RandomDesk(),
                Portfolio = RandomPortfolio()
            };
        }

        public String RandomDesk()
        {
            var index = (int)(this.RandNumberProvider.NextDouble()* Desks.Length);
            return Desks[index];
        }

        public string RandomIsin()
        {
            var index = (int)(this.RandNumberProvider.NextDouble()* ISINS.Length);
            return ISINS[index];
        }

        public string RandomPortfolio()
        {
             var index = (int)(this.RandNumberProvider.NextDouble()* Portfolios.Length);
            return Portfolios[index];
        }

        public string NextTradeId()
        {
            TradeIndex++;
            return TradeIndex.ToString();
        }
 
    }
}