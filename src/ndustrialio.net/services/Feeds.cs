using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


using com.ndustrialio.api.http;

namespace com.ndustrialio.api.services
{

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
	}
	
}