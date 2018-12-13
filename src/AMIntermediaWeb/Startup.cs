using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMIntermediaCore;

namespace AMIntermediaWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBackgroundProcessors();
            services.AddWebSocketManager();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseWebSockets (new WebSocketOptions(){
                KeepAliveInterval = TimeSpan.FromSeconds(60), 
                ReceiveBufferSize = 4 * 1024
            });

            var dashboardHandler = serviceProvider.GetService<DashboardRealtimeHandler>();
            var ordersPullingService = serviceProvider.GetService<OrdersPullingService>();
            var aggregationService = serviceProvider.GetService<AggregationService>();


            aggregationService.Start();
            ordersPullingService.Start();
            aggregationService.OrdersUpdateHandler = dashboardHandler.SendOrderUpdate;
            aggregationService.OrdersAdditionHandler = dashboardHandler.OrdersAdditionHandler;
            aggregationService.AxesUpdateHandler = dashboardHandler.SendAxeUpdate;
            
            app.MapWebSocketManager("/ws", dashboardHandler);
            
            /*
            app.UseHttpsRedirection();
            */
            app.UseMvc();
        }
    }
}
