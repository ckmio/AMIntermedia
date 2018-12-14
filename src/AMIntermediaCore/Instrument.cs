using System;
using Newtonsoft.Json.Linq;
namespace AMIntermediaCore
{
    public class Instrument 
    {
        public string Sector { get; set;}
        public string ISIN {get; set;}
        public string SecName {get; set;}
        public string GlobalSectorSupply {get; set;}
        public string GlobalSectorDemand {get; set;}
        public string GlobalSectorNetSupply {get; set;}
        public string CounterParty {get; set;}
        public string Desk{get; set;}
        public string Volume {get; set; }

        public string SecType {get; set;}
    }
}