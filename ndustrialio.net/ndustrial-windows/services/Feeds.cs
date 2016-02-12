using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

using com.ndustrialio.api.http;

namespace com.ndustrialio.api.services
{
    public class FeedType
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }
    }

	public class FeedData : ApiData
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("facility_id")]
        public int FacilityID { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("key")]
        public String FeedKey { get; set; }

        //[JsonProperty("routing_keys")]
        //public List<String> RoutingKeys { get; set; }

        [JsonProperty("token")]
        public String FeedToken { get; set; }

        [JsonProperty("timezone")]
        public String TimeZone { get; set; }

        [JsonProperty("feed_type")]
        public FeedType FeedType { get; set; }

        [JsonProperty("status")]
        public String Status { get; set; }

        [JsonProperty("created_at")]
        public String CreatedAt { get; set; }
    }


	public class Feeds : Service
	{
		public static String URI = "/feeds";
		
		
		public Feeds(Client client) : base(client) { }
		
		public List<FeedData> get(object id=null, Dictionary <String, String> parameters=null)
		{
			string uri = Feeds.URI;
			
            if (id != null)
            {
                uri += "/";
                uri += id;
            }

            Response response = this._get(new Request(uri, parameters:parameters));

            List<FeedData> ret = JsonConvert.DeserializeObject<List<FeedData>>(response.ToString());

            return ret;
        }
	}
	
}