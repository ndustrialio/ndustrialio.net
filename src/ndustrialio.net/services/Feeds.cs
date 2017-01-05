using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using com.ndustrialio.api.http;

namespace com.ndustrialio.api.services
{

    public class Feed
    {
        public string Key {get; set;}
        
        public string Token {get; set;}

        public string Timezone {get; set;}
    }

    public class Output
    {
        public int id {get; set;}

        public int feed_id {get; set;}

        public string key {get; set;}

        public string label {get; set;}

        public string status {get; set;}

        public int facility_id {get; set;}

        public bool is_logical {get; set;}

        public int output_type_id {get; set;}
    }

    public class OutputField
    {
        public int id {get; set;}

        public int output_id {get; set;}

        public string field_descriptor {get; set;}

        public string field_name {get; set;}        
        public string field_human_name {get; set;}

        public string label {get; set;}

        public bool is_totalizer {get; set;}        
        
        public string is_labeled {get; set;}

        public double scalar {get; set;}        
        
        public double divisor {get; set;}        
        public string units {get; set;}
    }

	public class FeedService : Service
	{
		public static String URI = "feeds"; 
		
		
		public FeedService(string client_id=null, string client_secret=null) : base(client_id, client_secret) { }

        public override string Audience
        {
            get{return "iznTb30Sfp2Jpaf398I5DN6MyPuDCftA";}
        }

        public override string BaseURL
        {
            get{return "https://feeds.api.ndustrial.io";}
        }

        public List<Feed> getFeeds(Dictionary <String, String> parameters=null)
        {

            APIResponse response = this.execute(new GET(uri:FeedService.URI, parameters:parameters));

            List<Feed> ret = JsonConvert.DeserializeObject<List<Feed>>(response.ToString());

            return ret;
        }

		public Feed getFeed(int id)
		{
			string uri = FeedService.URI + "/" + id;
			

            APIResponse response = this.execute(new GET(uri));

            Feed ret = JsonConvert.DeserializeObject<Feed>(response.ToString());

            return ret;
        }

        public Feed getFeed(string key)
        {
            string uri = FeedService.URI;

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"key", key}
            };


            APIResponse response = this.execute(new GET(uri: uri, parameters:requestParams));

            JObject feed = (JObject)JObject.Parse(response.ToString())["records"][0];

            Feed ret = new Feed();

            ret.Key = (string)feed["key"];
            ret.Timezone = (string)feed["timezone"];
            ret.Token = (string)feed["token"];


            return ret;
        }


        //  def getFieldDescriptors(self, feed_id, limit=100, offset=0, execute=True):

        // assert isinstance(feed_id, int)
        // assert isinstance(limit, int)
        // assert isinstance(offset, int)

        // params = {'limit': limit,
        //           'offset': offset}

        // return self.execute(GET('feeds/{}/fields'.format(feed_id)).params(params), execute=execute)

        public object getFieldDescriptors(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "fields"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()},
                {"offset", offset.ToString()}
            };

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;

                                    
        }

        public object getUnprovisionedFieldDescriptors(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "fields", "unprovisioned"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()},
                {"offset", offset.ToString()}
            };

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;
        }


        public List<Output> getFeedOutputs(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "outputs"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()},
                {"offset", offset.ToString()}
            };

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            PagedResponse<Output> ret = JsonConvert.DeserializeObject<PagedResponse<Output>>(response.ToString());

            return ret.records;
        }

        public List<OutputField> getOutputFields(int output_id)
        {
            object[] uriChunks = {"outputs", output_id, "fields"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            PagedResponse<OutputField> ret = JsonConvert.DeserializeObject<PagedResponse<OutputField>>(response.ToString());

            return ret.records;

        }

        public object getData(int output_id, string field_human_name, int window, DateTime time_start, DateTime? time_end=null)
        {
            object[] uriChunks = {"outputs", output_id, "fields", field_human_name, "data"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                // Convert to epoch-seconds
                {"timeStart", time_start.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds.ToString()},
            };

            if (time_end != null)
            {
                requestParams.Add("timeEnd", time_end.Value.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds.ToString());
            }

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;
        }

        public object getUnprovisionedData(int feed_id, string field_descriptor, int window, DateTime time_start, DateTime? time_end=null)
        {
            object[] uriChunks = {"feeds", feed_id, "fields", field_descriptor, "data"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                // Convert to epoch-seconds
                {"timeStart", time_start.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds.ToString()},
            };

            if (time_end != null)
            {
                requestParams.Add("timeEnd", time_end.Value.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds.ToString());
            }

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;
        }
	}
	
}