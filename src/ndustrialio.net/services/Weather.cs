using System;
using Newtonsoft.Json.Linq;
using com.ndustrialio.api.http;


namespace com.ndustrialio.api.services
{

    public class WeatherService : Service
    {
		public WeatherService(string client_id, string client_secret=null) : base(client_id, client_secret) { }

        public override string Audience
        {
            get{return "cFhOshImtabrVBHzPtwtOLYOT2Mp8IAh";}
        }

        public override string BaseURL
        {
            get{return "https://weather.api.ndustrial.io";}
        }

        public object getForecast(int location_id)
        {
            object[] uriChunks = {"locations", location_id, "forecast", "daily"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            dynamic ret = JArray.Parse(response.ToString());

            return ret;

        }

        public object getLocations()
        {
            object[] uriChunks = {"locations"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            dynamic ret = JArray.Parse(response.ToString());

            return ret;
        }

        public object getLocationInfo(int location_id)
        {
            object[] uriChunks = {"locations", location_id};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;
        }
    }


}