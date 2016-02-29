using System;
using com.ndustrialio.api;
using com.ndustrialio.api.ngest;
using com.ndustrialio.api.services;
using System.Collections.Generic;

namespace com.ndustrialio.test      
{
    class Program
    {
        public static String FEED_KEY = "ngest-geneva-mm";

        static void Main(string[] args)
        {

            NdustrialIoApi api = new NdustrialIoApi(accessToken: args[0]);

            // Instantiate ngestClient
            // args[0] is api token
            var ngestClient = new NgestClient(FEED_KEY, api);



            TimeSeriesData data = ngestClient.newTimeSeries();

            String testField = "test_field";

            // Add value to the TimeSeriesData object
            // These timestamps are assumed to be in the timezone specified by the feed
            data.addValue(DateTime.Parse("2015-09-24 10:45:00"), testField, 25);
            data.addValue(DateTime.Parse("2015-09-24 10:50:00"), testField, 30);
            data.addValue(DateTime.Parse("2015-09-24 10:55:00"), testField, 35);
            data.addValue(DateTime.Parse("2015-09-24 11:00:00"), testField, 40);

            // Send data to ndustrial.io
            ngestClient.sendData(data);
        }
    }
}
