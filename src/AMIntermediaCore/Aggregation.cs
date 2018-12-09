using System;
using System.Collections.Generic;
using  System.Linq;

namespace AMIntermediaCore
{
    public class Aggregation 
    {
        public string Id {get; set;}
        public Order OpenOrder {get; set; }

        public List<Axe> Axes {get; set; }
    }
}