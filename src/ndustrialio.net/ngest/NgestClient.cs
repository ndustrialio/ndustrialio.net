


using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using com.ndustrialio.api.services;
using System.Reflection;


namespace com.ndustrialio.api.ngest
{
	public class NgestClient
	{

        // private static String BASE_URL = "https://data.ndustrial.io/v1/";

		// private String _feedKey, _feedToken, _feedTimeZone;

        // private String _postURL;

        // private NdustrialIoApi _api;

        // public NgestClient(String feed_key, NdustrialIoApi api)
		// {
		// 	_feedKey = feed_key;

        //     _api = api;

		// 	getFeedInfo();

        //     Console.WriteLine("Feed key: " + _feedKey);
        //     Console.WriteLine("Feed timezone: " + _feedTimeZone);
        //     Console.WriteLine("Feed token: " + _feedToken);

        //     // Construct post URL
        //     _postURL = BASE_URL
        //         + _feedToken
        //         + "/ngest/"
        //         + _feedKey;
        // }

		// private void getFeedInfo()
		// {


        //     // Get info about our feed
		// 	List<FeedData> feedData =
		// 		_api.FEEDS.get(parameters: new Dictionary<String, String> { { "key", _feedKey } });

        //     if (feedData.Count == 0)
        //     {
        //         throw new UnregisteredFeedException("Feed with key " + _feedKey + " not does not exist in the ndustrial.io system! Please register your feed.");
        //     }

        //     _feedToken = feedData[0].FeedToken;
        //     _feedTimeZone = feedData[0].TimeZone;
		// }

        // public void sendData(TimeSeriesData data)
        // {
        //     List<String> dataToSend = data.getJSONData();

        //     // Ask the Ngest API service to send the data for us
        //     foreach (var d in dataToSend)
        //     {
        //         _api.NGEST.sendData(_feedToken, _feedKey, d);
        //     }


        // }

        // public TimeSeriesData newTimeSeries()
        // {
        //     return new TimeSeriesData(_feedKey, _feedTimeZone);
        // }

        // public void sendDataAsync(TimeSeriesData data)
        // {
        //     Thread thread = new Thread(() => sendData(data));
        //     thread.Start();
        // }

    }
}
