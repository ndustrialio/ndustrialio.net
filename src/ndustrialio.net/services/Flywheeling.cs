using System;
using System.Globalization;
using System.Collections.Generic;
using com.ndustrialio.api.http;
using Newtonsoft.Json.Linq;



namespace com.ndustrialio.api.services
{

    public class SetpointData<T> : Dictionary<DateTime, T>
    {
        public SetpointData(JObject json) : base()
        {
            foreach(var setpoint in json)
            {
                this.Add(DateTime.Parse(s: setpoint.Key, 
                                    provider: CultureInfo.CurrentCulture,
                                    styles: DateTimeStyles.AdjustToUniversal), setpoint.Value.ToObject<T>());
            }
        }
    }

    public class FlywheelingNode
    {
        private string _systemID, _edgeNodeID;


        public FlywheelingNode(JObject json)
        {
            _systemID = (string)json["system_id"];
            _edgeNodeID = (string)json["edge_node_id"];

        }

        public string SystemID
        {
            get { return _systemID; }
        }

        public string EdgeNodeID
        {
            get { return _edgeNodeID; }
        }
    }

    public class FlywheelingService : Service
    {
        public FlywheelingService(string client_id=null, string client_secret=null) : base(client_id, client_secret) {}

        public override string Audience
        {
            get {return "GvjVT0O7PO1biyzABeqInlodVbN9TsCf";}
        }

        public override string BaseURL
        {
            get {return "https://flywheeling.api.ndustrial.io";}
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

        public Dictionary<string, SetpointData<double>> getSchemesForSystem(string system_id)
        {
            object[] uriChunks = {"systems", system_id, "schemes", "data"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));


            var ret = new Dictionary<string, SetpointData<double>>();


            // Decode and package response data
            JObject responseData = JObject.Parse(response.ToString());



            foreach (var data in responseData.GetValue("data_by_zone").ToObject<JObject>())
            {
                ret.Add(data.Key, new SetpointData<double>(data.Value.ToObject<JObject>()));
            }

            return ret;
        }

        public Dictionary<string, SetpointData<bool>> getSetPointsForSystem(string system_id)
        {
            object[] uriChunks = {"systems", system_id, "runs", "data"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));


            var ret = new Dictionary<string, SetpointData<bool>>();


            // Decode and package response data
            JObject responseData = JObject.Parse(response.ToString());

            foreach (var data in responseData)
            {
                ret.Add(data.Key, new SetpointData<bool>(data.Value.ToObject<JObject>()));
            }

            return ret;
        }



        public SetpointData<bool> getSetPointsForZone(string zone_id)
        {
            object[] uriChunks = {"zones", zone_id, "runs", "data"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            var ret = new SetpointData<bool>(JObject.Parse(response.ToString()));

            return ret;
        }

        public Dictionary<int, List<FlywheelingNode>> getNodes()
        {
            object[] uriChunks = {"systems", "node"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));

            var responseData = JObject.Parse(response.ToString());

            int facility_id = (int)responseData["facility_id"];

            var ret = new Dictionary<int, List<FlywheelingNode>>();

            var nodeList = new List<FlywheelingNode>();

            ret.Add(facility_id, nodeList);

            foreach (var node in responseData["FlywheelingNodes"])
            {
                nodeList.Add(new FlywheelingNode((JObject)node));
            }

            return ret;

        } 
    }
}