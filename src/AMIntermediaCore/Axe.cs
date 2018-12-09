using System;

namespace AMIntermediaCore
{
    public class Axe 
    {
        public string Id { get; set;}
        public string ISIN {get; set;}
        public string CounterParty {get; set;}
        public string Desk{get; set;}
        public string BuyOrSell {get; set; }
        public string Currency {get; set;}
        public double? DiscountMargin {get; set;}
        public double? MidSwap {get; set;}
        public double? Zspread {get; set;}

        public string CDSBasis {get; set;}
        public double BenchSpread{get; set;}

        public string BenchIsin {get; set;}
        public double? CashPrice {get; set;}

        public double? YieldPrice {get; set;}
        public string Commentary {get; set;}

        public string FileEventId {get; set;}
        public DateTime CreationDate {get; set;}
        
    }
}