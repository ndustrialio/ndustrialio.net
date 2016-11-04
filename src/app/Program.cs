using System.Collections.Generic;
using System;
using com.ndustrialio.api.services;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
           string client_id = "o0Q9GPrsAvNsofW610VZkk6RMNnxdUwh";
           string client_secret= "UqyLe3XRkD6c7HqHgBuOmWiaXSc_DmzKzi1Z5aNdAOkKIC5A_u-1H2bSeaG6ZV4l";

           FeedService feeds = new FeedService(client_id: client_id,
                                                client_secret: client_secret);

            Dictionary<string, string> feedData = feeds.getFeeds(id:300);

            Console.WriteLine("Got feed ID 3000");
           
        }
    }
}
