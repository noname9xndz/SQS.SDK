using System;
using System.Runtime;
using System.Linq;
using System.Reflection;
using System.Text;
using Amazon.SQS;
using Noname.SQS.SDK.Client;
using Noname.SQS.SDK.Consumer;
using Noname.SQS.SDK.HealthChecks;
using Noname.SQS.SDK.Models;
using Noname.SQS.SDK.Process;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Noname.SQS.SDK
{
    public static class SQSIOC
    {
        public static SQSConfig sqsConfig = new SQSConfig();
        public static IServiceCollection RegisterSQS(this IServiceCollection services, IConfiguration configuration, string configKey)
        {
            configuration.GetSection(nameof(SQSConfig)).Bind(sqsConfig);
            services.Configure<SQSConfig>(options => configuration.GetSection(configKey).Bind(options));
            services.AddLogging(x => x.AddFilter("Microsoft", LogLevel.Warning));
            services.AddSingleton<IAmazonSQS>(x => SQSClientFactory.CreateClient(sqsConfig));
            services.AddSingleton<ISQSClient, SqsClient>();

            services.AddHealthChecks()
                .AddCheck<SQSHealthCheck>("SQS Health Check");
            return services;
        }

        public static IServiceCollection AddSQSConsumer(this IServiceCollection services)
        {
            services.AddScoped<ISQSConsumerService, SQSConsumerService>();
            return services;
        }

        public static void RegisterMessageProcessor(this IServiceCollection services, Assembly assembly)
        {
            string endwithInterface = "messageprocessor";
            var allCommandHandler = assembly.GetTypes().Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.Name.ToLower().EndsWith(endwithInterface));
            foreach (var type in allCommandHandler)
            {
                services.AddScoped(typeof(IMessageProcessor), type);
            }
        }
    }
}
