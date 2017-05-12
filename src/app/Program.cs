using System;
using com.ndustrialio.api.services;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            FlywheelingService service = new FlywheelingService();
            var fieldMetrics = service.getAvgForRoom("Room 11");

            Console.WriteLine("Maximum:" + fieldMetrics.metrics.maximum);
        }
    }
}
