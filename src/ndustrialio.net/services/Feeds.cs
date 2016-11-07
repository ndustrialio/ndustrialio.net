using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


using com.ndustrialio.api.http;

namespace com.ndustrialio.api.services
{

	public class FeedService : Service
	{
		public static String URI = "feeds"; 
		
		
		public FeedService(string client_id, string client_secret=null) : base(client_id, client_secret) { }

        public override string audience()
        {
            return "iznTb30Sfp2Jpaf398I5DN6MyPuDCftA";
        }

        public override string baseURL()
        {
            return "https://feeds.api.ndustrial.io";
        }

        public object getFeeds(Dictionary <String, String> parameters=null)
        {

            APIResponse response = this.execute(new GET(uri:FeedService.URI, parameters:parameters));

            dynamic ret = JArray.Parse(response.ToString());

            return ret;
        }

		public object getFeed(int id)
		{
			string uri = FeedService.URI + "/" + id;
			

            APIResponse response = this.execute(new GET(uri));

            dynamic ret = JObject.Parse(response.ToString());

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


        public object getFeedOutputs(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "outputs"};

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

        public object getOutputFields(int output_id)
        {
            object[] uriChunks = {"outputs", output_id, "fields"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;

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