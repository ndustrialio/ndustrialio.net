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

	public class FeedData
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


	public class FeedService : Service
	{
		public static String URI = "/feeds";
		
		
		public FeedService(string client_id, string client_secret=null) : base(client_id, client_secret) { }

        public override string audience()
        {
            return "iznTb30Sfp2Jpaf398I5DN6MyPuDCftA";
        }

        public override string baseURL()
        {
            return "https://feeds.api.ndustrial.io";
        }

		public FeedData getFeeds(object id=null, Dictionary <String, String> parameters=null)
		{
			string uri = FeedService.URI;
			
            if (id != null)
            {
                uri += "/";
                uri += id;
            }

            APIResponse response = this.execute(new GET(uri));

            Dictionary<string, string> ret = 
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(response.ToString());

            return ret;
        }


        //  def getFieldDescriptors(self, feed_id, limit=100, offset=0, execute=True):

        // assert isinstance(feed_id, int)
        // assert isinstance(limit, int)
        // assert isinstance(offset, int)

        // params = {'limit': limit,
        //           'offset': offset}

        // return self.execute(GET('feeds/{}/fields'.format(feed_id)).params(params), execute=execute)

        public FeedData getFieldDescriptors(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "fields"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()},
                {"offset", offset.ToString()}
            };

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            FeedData ret = 
                        JsonConvert.DeserializeObject<FeedData>(response.ToString());

            return ret;

                                    
        }
	}
	
}