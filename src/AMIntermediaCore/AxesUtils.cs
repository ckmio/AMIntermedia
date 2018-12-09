using System; 
using System.Collections.Generic; 
using System.Linq;

namespace AMIntermediaCore 
{
    public static class  AxesUtils 
    {
        public static string  GetString(Dictionary<string, object> cells, string label)
        {
            return (string)((cells.ContainsKey(label))? cells[label] : null);
        }

        public static object  GetObject(Dictionary<string, object> cells, string label)
        {
            return (cells.ContainsKey(label)? cells[label] : null);
        }

        public static double  GetDouble(Dictionary<string, object> cells, string label)
        {          
            return (double)(cells.ContainsKey(label)? cells[label] : double.NaN);
        }

        public static string  GetString(string[] cells, List<string> Headers, string label)
        {
            var index = Headers.FindIndex( header => header == label);
            return (index>=0 && cells.Length> index)? cells[index] : null;
        }

        public static double  GetDouble(string[] cells, List<string> Headers, string label)
        {
            var index = Headers.FindIndex( header => header == label);
            var doubleStr =  (index>=0 && cells.Length> index)? cells[index] : null; 
            double retVal    = double.NaN;
            if(double.TryParse(doubleStr, out retVal))
                return retVal;
            return double.NaN;
        }

        public static  string[] StringFields()
        {
            return new string []{"COUNTERPART","DESK","ISIN_CODE","BUY/SELL","CURRENCY","ASW","CDS_Basis","BENCH_ISIN","COMMENTARY"};
        }
        public  static string[] DoubleFields()
        {
            return new [] {"AMOUNT","CASH_PRICE","YIELD_PRICE","Z_SPREAD","BENCH_SPREAD","MID_SWAP","DISCOUNT_MARGING"};
        }

        public static Axe FromDictionary(Dictionary<string, object> cells)
        {
            return new Axe{
                    Id = System.Guid.NewGuid().ToString(),
                    ISIN = GetString(cells, "ISIN_CODE"),
                    CounterParty = GetString(cells, "COUNTERPART"),
                    Desk=GetString(cells, "DESK"),
                    BuyOrSell = GetString(cells, "BUY/SELL"),
                    Currency =GetString(cells, "CURRENCY"),
                    DiscountMargin = GetDouble(cells, "DISCOUNT_MARGING"),
                    MidSwap = GetDouble(cells, "MID_SWAP"),
                    Zspread = GetDouble(cells, "Z_SPREAD"),

                    CDSBasis = GetString(cells, "CDS_Basis"),
                    BenchSpread=GetDouble(cells, "BENCH_SPREAD"),

                    BenchIsin =GetString(cells, "BENCH_ISIN"),
                    CashPrice =GetDouble(cells, "CASH_PRICE"),

                    YieldPrice =GetDouble(cells, "YIELD_PRICE"),
                    Commentary =GetString(cells, "COMMENTARY"),

                    FileEventId = GetString(cells, "FILE_EVENT_ID"),
                    CreationDate = (GetObject(cells, "CR_DATE")!= null) ? ((DateTime)GetObject(cells, "CR_DATE")) : DateTime.Now
            };
            
        }
    }
}