using System;
using System.Timers;
using Ckmio;
using Microsoft.Extensions.Configuration;

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
         public string[] Sides = new string[]{ "BUY","SELL" };
         public string[] ISINS = new string[]{ "FR0011962398","FR0125218232","NL0000102317","SK4120011420", "XS0460357550" };
         public string[] Amounts = new string[]{ "100000","200000","300000","800000", "250000" };

          public Instrument[] Instruments = new Instrument[] {
             new Instrument{ Desk = "CORP", ISIN = "XS1051305974", SecName = "BANK NOVA SCOTIA 1.000% 02-04-19 [EUR]", SecType = "", Sector = "FINANCIAL", GlobalSectorSupply ="75 M",  GlobalSectorDemand ="63 M", GlobalSectorNetSupply ="12 M"  },
             new Instrument{ Desk = "CORP", ISIN = "IT0005028052", SecName = "MEDIOBANCA SPA 1.125% 17-06-19 [EUR]", SecType = "",  Sector = "FINANCIAL", GlobalSectorSupply ="75 M",  GlobalSectorDemand ="63 M", GlobalSectorNetSupply ="12 M"  },
             new Instrument{ Desk = "CORP", ISIN = "DE000A1AKHB8", SecName = "ALLIANZ SE 4.75% 22-07-19 [EUR]", SecType = "", Sector = "FINANCIAL", GlobalSectorSupply ="75 M",  GlobalSectorDemand ="63 M", GlobalSectorNetSupply ="12 M" },
             new Instrument{ Desk = "CORP", ISIN = "FR0012146777", SecName = "SANOFI 1.125% 10-03-22 [EUR]", SecType = "", Sector = "HEALTHCARE", GlobalSectorSupply ="45 M",  GlobalSectorDemand ="40 M", GlobalSectorNetSupply ="5 M" },
             new Instrument{ Desk = "CORP",  ISIN = "BE6285452460", SecName = "ANHEUSER-BUSCH 0.875% 17-03-22 [EUR]", SecType = "", Sector = "CONSUMER NON-CYCLICAL", GlobalSectorSupply ="65 M",  GlobalSectorDemand ="68 M", GlobalSectorNetSupply ="-3 M"  },
              new Instrument{ Desk = "EMERGING", ISIN = "SK4120011420", SecName = "", SecType = "", Sector = "EMERGING",GlobalSectorSupply ="25 M",  GlobalSectorDemand ="23 M", GlobalSectorNetSupply ="2 M"  },
             new Instrument{ Desk = "EMERGING",  ISIN = "XS0460357550", SecName = "", SecType = "", Sector = "EMERGING",GlobalSectorSupply ="25 M",  GlobalSectorDemand ="23 M", GlobalSectorNetSupply ="2 M"  },
             new Instrument{ Desk = "GOVIES",  ISIN = "FR0010810069", SecName = "OAT 0 %  25-04-2025 [EUR]", SecType = "", Sector = "SOVEREIGN",GlobalSectorSupply ="375 M",  GlobalSectorDemand ="230 M", GlobalSectorNetSupply ="145 M"  },
             new Instrument{ Desk = "GOVIES",  ISIN = "NL0000102275", SecName = "NETHER 3.75 15- 01-23 [EUR]", SecType = "", Sector = "SOVEREIGN",GlobalSectorSupply ="375 M",  GlobalSectorDemand ="230 M", GlobalSectorNetSupply ="145 M"  },
             new Instrument{ Desk = "GOVIES",  ISIN = "FR0013238268", SecName = "FRANCE O.A.T. 0.100% 01-03-28 [EUR]", SecType = "", Sector = "SOVEREIGN",GlobalSectorSupply ="375 M",  GlobalSectorDemand ="230 M", GlobalSectorNetSupply ="145 M"  },
         };

        public static int TradeIndex {get; private set;}

        public OrdersPullingService()
        {
             IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
            .Build();
            this.OrdersStream = config["AMIntermediaServices:OrdersStreamName"];;
            this.IntervalInMilliseconds =  int.Parse(config["AMIntermediaServices:NewOrdersPullInterval"]);
            this.PullingTimer = new System.Timers.Timer();
            this.PullingTimer.Interval = int.Parse(config["AMIntermediaServices:NewOrdersPullInterval"]);
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
            var instr = RandomIsin();
            return new Order {
                Id = Guid.NewGuid().ToString(),
                TradeId = NextTradeId(),
                ISIN = instr.ISIN,
                BuyOrSell = RandomBuyOrSell(),
                Desk = instr.Desk,
                SecName = instr.SecName,
                Amount = RandomInstr(),
            };
        }

        public String RandomDesk()
        {
            var index = (int)(this.RandNumberProvider.NextDouble()* Desks.Length);
            return Desks[index];
        }

        public String RandomInstr()
        {
            var index = (int)(this.RandNumberProvider.NextDouble()* Amounts.Length);
            return Amounts[index];
        }

        public String RandomBuyOrSell()
        {
            var index = (int)(this.RandNumberProvider.NextDouble()* Sides.Length);
            return Sides[index];
        }

        public Instrument RandomIsin()
        {
            var index = (int)(this.RandNumberProvider.NextDouble()* Instruments.Length);
            return Instruments[index];
        }

        public string NextTradeId()
        {
            TradeIndex++;
            return TradeIndex.ToString();
        }
 
    }
}