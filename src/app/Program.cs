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

            WeatherService weather = new WeatherService(client_id: client_id,
                                                        client_secret: client_secret);

            dynamic feedData = feeds.getFeed(id:300);

            string key = feedData.key;

            Console.WriteLine("Got feed ID 3000, key " + key);

            dynamic fields = feeds.getFieldDescriptors(feed_id:300);

            Console.WriteLine("Got " + fields.records.Count + " fields for id 300");

            dynamic chicago_forecast = weather.getForecast(location_id: 14);

            foreach (dynamic day in chicago_forecast)
            {
                Console.WriteLine(day.date + ": High: "+day.forecast_high + ", Low: " + day.forecast_low);
            }
           
        }
    }
}
