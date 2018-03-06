using System;
using System.Collections.Generic;
using com.ndustrialio.api.services;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new FeedService();

            var metrics = service.getFieldMetrics(
                new List<int> {6695, 6713, 6734, 6752, 6696, 6714, 6715, 6735, 6753, 6697},
                new List<string>{"Temperature"});

            Console.WriteLine(metrics.metrics);
        }
    }
}
