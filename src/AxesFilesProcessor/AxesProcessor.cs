using System; 
using System.IO;
using System.Threading;
using Ckmio;

namespace AMIntermedia.AxesFilesProcessor
{
    public class AxesProcessor
    {
        public string IncomingFilesDirectoryPath {get; set;}
        public string ProcessingDirectoryPath {get; set;}
        public string PublishingStreamId {get; set;}
        private ManualResetEvent quit = new ManualResetEvent(false);
        public CkmioClient MioClient {get; set;}
        public AxesProcessor(string incomingPath, string processingPath, string publishingStreamId)
        {
            this.IncomingFilesDirectoryPath = incomingPath;
            this.ProcessingDirectoryPath = processingPath;
            this.PublishingStreamId = publishingStreamId;
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
                MioClient.SendToStream("new-axes", new { Name = e.Name, FullPath = e.FullPath, ChangeType = e.ChangeType, content = line });
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
                MioClient.SendToStream("new-axes", new { Name = e.Name, FullPath = e.FullPath, ChangeType = e.ChangeType, content = line });
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