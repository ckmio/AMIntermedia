using System;
using Ckmio;

namespace AMIntermedia.AxesFilesProcessor
{
    class Program
    {
        static void Main(string[] args)
        {          
            string incomingFilesDirectory ="/Users/asissokho/testdir/inputs";
            string processingFileDirectory ="/Users/asissokho/testdir/processing";
            string publishingStreamId = "new-axes"; 
            var axesProcessor = new AxesProcessor(incomingFilesDirectory, processingFileDirectory, publishingStreamId);
            axesProcessor.Start();
            Console.WriteLine("Incoming File processor correctly set!");
            Console.ReadLine();
        }
    }
}
