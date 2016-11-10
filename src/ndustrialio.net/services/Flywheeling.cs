using System;
using System.Collections.Generic;
using com.ndustrialio.api.http;
using Newtonsoft.Json.Linq;



namespace com.ndustrialio.api.services
{

    public class SetpointData : Dictionary<DateTime, bool>
    {
        public SetpointData(JObject json) : base()
        {
            foreach(var setpoint in json)
            {
                this.Add(DateTime.Parse(setpoint.Key), setpoint.Value.ToObject<bool>());
            }
        }
    }

    public class FlywheelingService : Service
    {
        public FlywheelingService(string client_id=null, string client_secret=null) : base(client_id, client_secret) {}

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

        public Dictionary<string, SetpointData> getSetPointsForSystem(string system_id)
        {
            object[] uriChunks = {"systems", system_id, "runs", "data"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));


            var ret = new Dictionary<string, SetpointData>();


            // Decode and package response data
            JObject responseData = JObject.Parse(response.ToString());

            foreach (var data in responseData)
            {
                ret.Add(data.Key, new SetpointData(data.Value.ToObject<JObject>()));
            }

            return ret;
        }

        public SetpointData getSetPointsForZone(string zone_id)
        {
            object[] uriChunks = {"zones", zone_id, "runs", "data"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            SetpointData ret = new SetpointData(JObject.Parse(response.ToString()));

            return ret;
        }
    }
}