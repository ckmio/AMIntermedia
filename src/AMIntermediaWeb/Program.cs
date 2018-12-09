using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AMIntermediaWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string axesStreamName = "new-axes";
            string ordersStreamName = "new-orders";
            new AMIntermediaCore.AggregationService(axesStreamName, ordersStreamName).Start();
            new AMIntermediaCore.OrdersPullingService(ordersStreamName, 30000).Start();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
