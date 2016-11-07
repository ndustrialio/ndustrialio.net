using System;
using com.ndustrialio.api.http;
using Newtonsoft.Json.Linq;



namespace com.ndustrialio.api.services
{

    // In case we ever need it
    // public class SetpointData : JObject
    // {
    //     public void add(DateTime timestamp, bool value)
    //     {

    //     }

    //     public void add(int timestamp, bool value)
    //     {

    //     }
    // }

    public class FlywheelingService : Service
    {
        public FlywheelingService(string client_id, string client_secret=null) : base(client_id, client_secret) {}

        public override string audience()
        {
            return "GvjVT0O7PO1biyzABeqInlodVbN9TsCf";
        }

        public override string baseURL()
        {
            return "https://flywheeling.api.ndustrial.io";
        }

        public object getRuns()
        {
            object[] uriChunks = {"runs"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            dynamic ret = JArray.Parse(response.ToString());

            return ret;
        }

        public object getRun(int run_id)
        {
            object[] uriChunks = {"runs", run_id};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            dynamic ret = JArray.Parse(response.ToString());

            return ret;
        }

        public object createRun(int facility_id, string solver_type, string name, DateTime ran_at)
        {
            object[] uriChunks = {"runs"};

            JObject body = new JObject 
            {
                {"facility_id", facility_id.ToString()},
                {"solver_type", solver_type},
                {"name", name},
                {"ran_at", ran_at.ToString("yyyy-MM-dd HH:mm:ss")}
            };

            APIResponse response = this.execute(new POST(uri: String.Join("/", uriChunks), 
                                                        body: body.ToString()));

            dynamic ret = JObject.Parse(response.ToString());

            return ret;

        }
    }
}