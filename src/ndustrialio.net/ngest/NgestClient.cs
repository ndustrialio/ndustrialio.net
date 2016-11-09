


using System;
using System.Collections.Generic;
using com.ndustrialio.api.services;
using System.Threading.Tasks;
using com.ndustrialio.api.http;


namespace com.ndustrialio.api.ngest
{
	public class NgestClient
	{

		private String _feedKey, _feedToken;

        private TimeZoneInfo _timeZone;

        private String _postURL;


        public NgestClient(string feed_key, string feed_token, string feed_timezone)
		{
			_feedKey = feed_key;
            _feedToken = feed_token;
            // 
            _timeZone = mapTimezones(feed_timezone);

        }


        private TimeZoneInfo mapTimezones(string feed_timezone)
        {
            switch(feed_timezone)
            {
                case "UTC":
                    return TimeZoneInfo.Utc;
                case "US/Eastern":
                case "America/New_York":
                    return TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
                case "US/Central":
                case "America/Chicago":
                    return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                case "US/Mountain":
                case "America/Denver":
                    return TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time");
                case "US/Pacfic":
                case "America/Los_Angeles":
                    return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                default:
                    throw new Exception("Timezone " + feed_timezone + " not supported by .NET client!");
                
            }
        }

        public void sendData(TimeSeriesData data)
        {
            List<String> dataToSend = data.getJSONData();

            NgestService ngest = new NgestService();

            // Ask the Ngest API service to send the data for us
            foreach (var d in dataToSend)
            {
                ngest.sendData(_feedToken, _feedKey, d);
            }


        }

        public async Task<APIResponse[]> sendDataAsync(TimeSeriesData data)
        {
            List<String> dataToSend = data.getJSONData();

            NgestService ngest = new NgestService();

            
            var response_tasks = new List<Task<APIResponse>>();

            // Ask the Ngest API service to send the data for us
            // Asyncrhonously
            foreach (var d in dataToSend)
            {
                response_tasks.Add(ngest.sendDataAsync(_feedToken, _feedKey, d));
            }
            var ret = await Task.WhenAll(response_tasks);    

            return ret;
        }

        public TimeSeriesData newTimeSeries()
        {
            return new TimeSeriesData(_feedKey, _timeZone);
        }

    }
}
