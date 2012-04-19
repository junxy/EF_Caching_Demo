using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFTracingProvider;
using EFCachingProvider;
using EFCachingProvider.Caching;
using EF4_Caching_Demo.Data;

namespace EF4_Caching_Demo
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


            for (int i = 0; i < 30; ++i)
            {
                Console.WriteLine();
                Console.WriteLine("*** Pass #{0}...", i);
                Console.WriteLine();
                using (var context = new ExtendedNorthwindEntities())
                {
                    // set up caching
                    context.Cache = cache;
                    context.CachingPolicy = cachingPolicy;

                    Console.WriteLine("Loading customer...");
                    var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                    Console.WriteLine("Customer name: {0}", cust.ContactName);
                    Console.WriteLine("Loading orders...");
                    cust.Orders.Load();
                    Console.WriteLine("Order count: {0}", cust.Orders.Count);
                }
            }

            Console.WriteLine();


        }
    }
}
