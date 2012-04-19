using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using EF_Caching_Demo.Data;
using EFTracingProvider;
using EFCachingProvider;
using EFCachingProvider.Caching;

namespace EF_Caching_Demo
{
    class Program
    {
        static void Main(string[] args)
        {

            EFTracingProviderConfiguration.RegisterProvider();
            EFCachingProviderConfiguration.RegisterProvider();

            ICache cache = new InMemoryCache();
            CachingPolicy cachingPolicy = CachingPolicy.CacheAll;

            // log SQL from all connections to the console
            EFTracingProviderConfiguration.LogToConsole = true;

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine();
                Console.WriteLine("*** Pass #{0}...", i);
                Console.WriteLine();
                using (var nDb = new NorthwindEntities())
                {
                    nDb.Cache = cache;
                    nDb.CachingPolicy = cachingPolicy;
                    
                    var emp = nDb.Customers.First(x => x.CustomerID == "ALFKI");

                    Console.WriteLine(nDb.Customers.First(x => x.CustomerID == "ALFKI").ContactName);
                    Console.WriteLine(nDb.Customers.First(x => x.CustomerID == "ALFKI").ContactName);
                    Console.WriteLine(nDb.Customers.AsNoTracking().First(x => x.CustomerID == "ALFKI").Orders.Count());

                }

            }
            
        }
    }
}
