using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using com.ndustrialio.api.http;
using System.Collections;

namespace com.ndustrialio.api.services
{

    public class FieldMetrics
    {
        public class Metrics
        {
            public double mean {get; set;}
            public double minimum {get; set;}
            public double maximum {get; set;}
            public double standard_deviation {get; set;}

        }

        public class Parameters
        {
            public string time {get; set;}
            public int stale_seconds {get; set;}

            public string human_time {get; set;}
        }
        public Metrics metrics;
        public Parameters parameters;
    }

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

    public class DataResponse : IEnumerable<Tuple<DateTime, string>>
    {
        public int Count {get; set;}

        public bool HasMore {get; set;}

        public string NextPageURL {get; set;}

        public List<Tuple<DateTime, string>> Records {get; set;}

        public DataResponse(JObject apiResponse) 
        {
            Count = (int)apiResponse["meta"]["count"];

            HasMore = (bool)apiResponse["meta"]["has_more"];

            NextPageURL = (string)apiResponse["meta"]["next_page_url"];

            List<JObject> recordsList = apiResponse["records"].ToObject<List<JObject>>();

            Records = new List<Tuple<DateTime, string>>();

            foreach (var data in recordsList)
            {
                Records.Add(new Tuple<DateTime, string>(DateTime.Parse(s: data["event_time"].ToObject<string>(), 
                                    provider: CultureInfo.CurrentCulture,
                                    styles: DateTimeStyles.AdjustToUniversal), 
                                    data["value"].ToObject<string>()));
            }
        }

        public IEnumerator<Tuple<DateTime, string>> GetEnumerator()
        {
            foreach(var data in Records)
            {
                yield return data;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
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

            // TODO: fix when feed doesn't exist
            JObject feed = (JObject)JObject.Parse(response.ToString())["records"][0];

            Feed ret = new Feed();

            ret.Key = (string)feed["key"];
            ret.Timezone = (string)feed["timezone"];
            ret.Token = (string)feed["token"];


            return ret;
        }

        public JObject getFieldDescriptors(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "fields"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()},
                {"offset", offset.ToString()}
            };

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));


            return JObject.Parse(response.ToString());

                                    
        }

        public JObject getUnprovisionedFieldDescriptors(int feed_id, int limit=100, int offset=0)
        {
            object[] uriChunks = {FeedService.URI, feed_id, "fields", "unprovisioned"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                {"limit", limit.ToString()},
                {"offset", offset.ToString()}
            };

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            

            return JObject.Parse(response.ToString());
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

        public DataResponse getData(int output_id, string field_human_name, int window, DateTime time_start, DateTime? time_end=null)
        {
            object[] uriChunks = {"outputs", output_id, "fields", field_human_name, "data"};

            Dictionary<string, string> requestParams = new Dictionary<string, string>()
            {
                // Convert to epoch-seconds
                {"timeStart", time_start.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds.ToString()},
                {"window", window.ToString()}
            };

            if (time_end != null)
            {
                requestParams.Add("timeEnd", time_end.Value.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalSeconds.ToString());
            }

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks), 
                                                    parameters: requestParams));

            return new DataResponse(JObject.Parse(response.ToString()));
        }

        public JObject getUnprovisionedData(int feed_id, string field_descriptor, int window, DateTime time_start, DateTime? time_end=null)
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

            return JObject.Parse(response.ToString());

        }

        public FieldMetrics getFieldMetrics(List<int> output_ids, List<string> labels)
        {
            var body = JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                {"output_id_list", output_ids},
                {"labels", labels}
            });

            APIResponse response = this.execute(new POST(uri:"metrics/fieldDataMetrics",
                                                        body: body));

            return JsonConvert.DeserializeObject<FieldMetrics>(response.ToString());
        }
	}
	
}