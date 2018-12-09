using System; 
using Ckmio;

namespace AMIntermedia.AxesFilesProcessor{

    public class CsvFileEvent
    {
        public string FileName {get; set;}
        public DateTime LastModifiedDate {get; set;}

        public CsvFileContent CsvFileContent {get; set;}

        public CsvFileEvent(string fileName, DateTime lastModifiedDate)
        {
            this.FileName = fileName; 
            this.LastModifiedDate = lastModifiedDate;
        }


        public void DebugFileContent()
        {
            System.Console.WriteLine($"Content of {FileName} \n {System.IO.File.ReadAllLines(FileName)}");
        }

    }
}