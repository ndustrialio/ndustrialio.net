using System;
using com.ndustrialio.api.services;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            FeedService feeds = new FeedService();

            var response = feeds.getData(1047, "Energy_Real_In", 60, new DateTime(2017, 04, 11, 2, 00, 00));

            foreach(var data in response)
            {
                Console.WriteLine(data.Item1.ToString() + ": " + data.Item2);
            }

        }
    }
}
