using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AMIntermediaCore
{
    public class Order 
    {
        public string Id {get; set;}

        public string ISIN {get; set;}
        public string TradeId {get; set; }
         public string TraderId {get; set; }

         public string SecName {get; set; }
         public string BuyOrSell {get; set;}

         public string Currency {get; set;}

         public string Amount {get; set;}
        public string Portfolio {get; set;}
        public string GoP {get; set;}

        public string ProfitCenter {get; set;}
        public string Desk {get; set;}

        public List<Axe> Axes {get; set; }

        public static Order FromJObject(JObject jObj ){
            return jObj.ToObject<Order>();
        }
    }
}

