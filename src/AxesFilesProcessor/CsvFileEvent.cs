using System; 
using Ckmio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AMIntermedia.AxesFilesProcessor{

    public class CsvFileEvent
    {
        public static char Comma = ',';
        public string FileFullPath {get; set;}
        public DateTime LastModifiedDate {get; set;}

        public List<string> Headers {get; set;}
        public List<Dictionary<string, object> > Lines {get; set;}

        public CsvFileEvent(string fileName, DateTime lastModifiedDate)
        {
            this.FileFullPath = fileName; 
            this.LastModifiedDate = lastModifiedDate;
        }



        public void DebugFileContent()
        {
            System.Console.WriteLine($"Content of {FileFullPath} \n {System.IO.File.ReadAllLines(FileFullPath)}");
        }

        public static CsvFileEvent FromFile(string fileFullPath)
        {
            CsvFileEvent fileEvent = new CsvFileEvent(fileFullPath, DateTime.Now){
                Lines = new List<Dictionary<string, object> >(),
                FileFullPath = fileFullPath
            };
            fileEvent.ReadCsvContent();
            System.Console.WriteLine($"Content of {fileFullPath} \n {System.IO.File.ReadAllText(fileFullPath)}");
            return fileEvent;
        }

        private void ReadCsvContent()
        {
            var nextLines = ReadHeaderStep(FileFullPath);
            foreach(var line in nextLines)
            {
                var curatedLines = line.Trim();
                var cells = curatedLines.Split(Comma, StringSplitOptions.RemoveEmptyEntries);
                NewLineFromCells(cells);
            }
        }

        private IEnumerable<string> ReadHeaderStep(string fileFullPath)
        {
            string[] allLines = System.IO.File.ReadAllLines(fileFullPath);
            int headerIndex = 0; 
            for( ; headerIndex< allLines.Length; headerIndex++){
                var headerCandidate = allLines[headerIndex].Trim();
                if(headerCandidate == String.Empty) continue;
                this.Headers = headerCandidate.Split(Comma, StringSplitOptions.RemoveEmptyEntries).ToList();
                break;
            }
            return allLines.Skip(headerIndex);
        }

        private void NewLineFromCells(string[] cells)
        {
            Dictionary<string, object> line = new Dictionary<string, object>();
            foreach(var field in StringFields())
                line.Add(field, GetString(cells, field));
            foreach(var field in DoubleFields())
                line.Add(field, GetDouble(cells, field));
            this.Lines.Add(line);
        }

        private string  GetString(string[] cells, string label)
        {
            var index = Headers.FindIndex( header => header == label);
            return (index>=0 && cells.Length> index)? cells[index] : null;
        }

         private double  GetDouble(string[] cells, string label)
        {
            var index = Headers.FindIndex( header => header == label);
            var doubleStr =  (index>=0 && cells.Length> index)? cells[index] : null; 
            double retVal    = double.NaN;
            if(double.TryParse(doubleStr, out retVal))
                return retVal;
            return double.NaN;
        }

        public  string[] StringFields()
        {
            return new string []{"COUNTERPART","DESK","ISIN_CODE","BUY/SELL","CURRENCY","ASW","CDS_Basis","BENCH_ISIN","COMMENTARY"};
        }
        public  string[] DoubleFields()
        {
            return new [] {"AMOUNT","CASH_PRICE","YIELD_PRICE","Z_SPREAD","BENCH_SPREAD","MID_SWAP","DISCOUNT_MARGING"};
        }

    }
}