using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using com.ndustrialio.api.http;


namespace com.ndustrialio.api.services
{

    public class WeatherLocation
    {
        public int id {get; set;}
        public string city {get; set;}
        public string state {get; set;}
        public string search_key {get; set;}
        public double latitude {get; set;}
        public double longitude {get; set;}
        public string timezone {get; set;}

    }

    public class WeatherService : Service
    {
		public WeatherService(string client_id=null, string client_secret=null) : base(client_id, client_secret) { }

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

        public List<WeatherLocation> getLocations()
        {
            object[] uriChunks = {"locations"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            List<WeatherLocation> ret = JsonConvert.DeserializeObject<List<WeatherLocation>>(response.ToString());

            return ret;
        }

        public WeatherLocation getLocationInfo(int location_id)
        {
            object[] uriChunks = {"locations", location_id};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            WeatherLocation ret = JsonConvert.DeserializeObject<WeatherLocation>(response.ToString());

            return ret;
        }
    }


}