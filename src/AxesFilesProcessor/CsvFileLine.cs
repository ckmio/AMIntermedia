using System; 
using System.Collections.Generic;
using System.Text;
using Ckmio;

namespace AMIntermedia.AxesFilesProcessor{

    public class CsvFileLine
    {
        public List<String> Contents; 
        public Dictionary<String, int> Headers;

        public String CsvValues()
        {
            StringBuilder sb = new StringBuilder();
            Contents.ForEach((val)=> {
                sb.Append(val);
                sb.Append(";"); });
            return sb.ToString(0, sb.Length -1);

        }

        public String Cell(string header)
        {
            return Contents[Headers[header]];
        }
    }
}