using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using AMIntermediaCore;

namespace AMIntermediaWeb
{
    public static class SocketsUtilsExtensions 
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, 
                                                              PathString path,
                                                              WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketManager>();

            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IServiceCollection AddBackgroundProcessors(this IServiceCollection services)
        {
            string axesStreamName = "new-axes";
            string ordersStreamName = "new-orders";
            services.AddSingleton<AggregationService>(new AggregationService(axesStreamName, ordersStreamName));
            services.AddSingleton<OrdersPullingService>(new OrdersPullingService(ordersStreamName, 30000));
            return services;
        }

    }
}