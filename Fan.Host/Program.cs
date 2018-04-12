using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Fan.Grains;
using Ray.MongoDb;
using Ray.RabbitMQ;
using Fan.IGrains;
using Ray.Core.Message;
using Orleans;
using System.Net;
using Fan.Grain.Account;
using Fan.Grains.Account;
using Orleans.Configuration;

namespace Fan.Host
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }
        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Input any key to stop");
                Console.ReadLine();
                await host.StopAsync();
                Console.WriteLine("Input any key to exit");
                Console.ReadLine();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
        private static async Task<ISiloHost> StartSilo()
        {
            var siloAddress = IPAddress.Loopback;

            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(AccountDb).Assembly).WithReferences())
                .ConfigureServices((context, servicecollection) =>
                {
                    servicecollection.AddSingleton<ISerializer, ProtobufSerializer>();//注册序列化组件
                    servicecollection.AddMongoDb();//注册MongoDB为事件库
                    servicecollection.AddRabbitMQ<MessageInfo>();//注册RabbitMq为默认消息队列
                })
                .Configure<MongoConfig>(c =>
                {
                    c.SysStartTime = DateTime.Now;
                    c.Connection = "mongodb://127.0.0.1:27017";
                })
                .Configure<RabbitConfig>(c =>
                {
                    c.UserName = "guest";
                    c.Password = "guest";
                    c.Hosts = new[] { "127.0.0.1:5672" };
                    c.MaxPoolSize = 100;
                    c.VirtualHost = "/";
                })
               .ConfigureLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Warning));

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
