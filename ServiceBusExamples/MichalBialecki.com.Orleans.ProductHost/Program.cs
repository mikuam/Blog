using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace MichalBialecki.com.Orleans.StoresHost
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static SiloHost siloHost;

        static void Main(string[] args)
        {
            // Orleans should run in its own AppDomain, we set it up like this
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup()
                {
                    AppDomainInitializer = InitSilo
                });

            DoSomeClientWork();

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            // We do a clean shutdown in the other AppDomain
            hostDomain.DoCallBack(ShutdownSilo);
        }

        static void DoSomeClientWork()
        {
            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            var productRating = GrainClient.GrainFactory.GetGrain<ProductGrainInterfaces.IProductRatingGrain>(0);
            Console.WriteLine("\n\n{0}\n\n", productRating.GetRating().Result);
        }

        static void InitSilo(string[] args)
        {
            siloHost = new SiloHost(System.Net.Dns.GetHostName());
            // The Cluster config is quirky and weird to configure in code, so we're going to use a config file
            siloHost.ConfigFileName = "OrleansConfiguration.xml";

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node", siloHost.Name, siloHost.Type));

        }

        static void ShutdownSilo()
        {
            if (siloHost != null)
            {
                siloHost.Dispose();
                GC.SuppressFinalize(siloHost);
                siloHost = null;
            }
        }
    }
}
