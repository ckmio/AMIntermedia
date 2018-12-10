using System;
using Ckmio;
using AMIntermediaCore;
namespace AMIntermedia.AxesFilesProcessor
{
    class Program
    {
        static void Main(string[] args)
        { 
            var axesProcessor = new AxesProcessor();
            axesProcessor.Start();
            Console.WriteLine("Incoming File processor correctly set!");
            Console.ReadLine();
        }
    }
}
