using System; 
using System.IO;
using System.Threading;
using Ckmio;
using Microsoft.Extensions.Configuration;

namespace AMIntermedia.AxesFilesProcessor
{
    public class AxesProcessor
    {
        public string IncomingFilesDirectoryPath {get; set;}
        public string ProcessingDirectoryPath {get; set;}

        public string AxesFilesFilter {get; set;}

        public string AxesStreamName {get; set;}
        private ManualResetEvent quit = new ManualResetEvent(false);
        public CkmioClient MioClient {get; set;}
        public AxesProcessor()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
            .Build();
            this.AxesStreamName = config["AMIntermediaServices:AxesStreamName"];;
            this.IncomingFilesDirectoryPath = config["AMIntermediaServices:AxesIncomingFilesDirectory"];
            this.ProcessingDirectoryPath = config["AMIntermediaServices:AxesProcessingLogDirectory"];
            this.AxesFilesFilter = config["AMIntermediaServices:AxesFilesFilter"];
            this.MioClient = new CkmioClient( "community-test-key",
            "community-test-secret", 
            "Producer", "xpassw0rd");
            this.MioClient.Start();
        }

        public void Start()
        {
            if(CheckConfiguration())
            {
                new Thread(WatchIncomingFilesDirectory).Start();
            }
            else
            {
                throw new Exception("Configuratin is incorrect");
            }

        }

        public void Stop()
        {
            quit.Set();

        }

        public bool CheckConfiguration()
        {
            return Directory.Exists(IncomingFilesDirectoryPath) && Directory.Exists(ProcessingDirectoryPath);
        }

        public void CsvFileEventHandler(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Name : "+ e.Name);
            Console.WriteLine("ChangeType : "+ e.ChangeType);
            Console.WriteLine("FullPath : "+ e.FullPath);
            var fileEvent = CsvFileEvent.FromFile(e.FullPath);
            foreach(var line in fileEvent.Lines)
            {
                MioClient.SendToStream(AxesStreamName,new { Name = e.Name, FullPath = e.FullPath, ChangeType = e.ChangeType, content = line });
            }
        }

        public void RenamedCsvFileEventHandler(object sender, RenamedEventArgs e)
        {
            Console.WriteLine("Name : "+ e.Name);
            Console.WriteLine("ChangeType : "+ e.ChangeType);
            Console.WriteLine("OldPath : "+ e.OldFullPath);
            Console.WriteLine("FullPath : "+ e.FullPath);
            var fileEvent = CsvFileEvent.FromFile(e.FullPath);
            foreach(var line in fileEvent.Lines)
            {
                MioClient.SendToStream(AxesStreamName, new { Name = e.Name, FullPath = e.FullPath, ChangeType = e.ChangeType, content = line });
            }
        }

        private void WatchIncomingFilesDirectory(){
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = IncomingFilesDirectoryPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess;
            /*
            watcher.Filter = "*.*";
            */
            watcher.Filter = "NAT_FI.*";
            watcher.Changed += new FileSystemEventHandler(CsvFileEventHandler);
            watcher.Renamed += new RenamedEventHandler(RenamedCsvFileEventHandler);
            watcher.EnableRaisingEvents = true;
            
        }
    }

}