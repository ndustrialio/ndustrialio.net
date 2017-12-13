using System;
using System.Globalization;
using System.Collections.Generic;
using com.ndustrialio.api.http;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



namespace com.ndustrialio.api.services
{

    public class Sensor
    {
        public int sensor_id {get; set;}
        public string id {get; set;}
        public string area_id {get; set;}
        public string sensor_type {get; set;}
        public string label {get; set;}
        public string created_at {get; set;}
        public string updated_at {get; set;}
    }

    public class SetpointData : Dictionary<DateTime, string>
    {
        public SetpointData(JObject json) : base()
        {
            foreach(var setpoint in json)
            {
                this.Add(DateTime.Parse(s: setpoint.Key, 
                                    provider: CultureInfo.CurrentCulture,
                                    styles: DateTimeStyles.AdjustToUniversal), setpoint.Value.ToObject<string>());
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

        public Tuple<string, Dictionary<string, SetpointData>> getSchemesForSystem(string system_id)
        {
            object[] uriChunks = {"systems", system_id, "schemes", "data"};

            APIResponse response = this.execute(new GET(uri: String.Join("/", uriChunks)));


            var zone_data = new Dictionary<string, SetpointData>();


            // Decode and package response data
            JObject responseData = JObject.Parse(response.ToString());

            string scheme_output_type = responseData.GetValue("scheme_output_type").ToObject<string>();

            foreach (var data in responseData.GetValue("data_by_zone").ToObject<JObject>())
            {
                zone_data.Add(data.Key, new SetpointData(data.Value.ToObject<JObject>()));
            }

            return Tuple.Create(scheme_output_type, zone_data);
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

            var ret = new SetpointData(JObject.Parse(response.ToString()));

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


        public Tuple<List<int>, List<string>> getSensorsForRoom(string room_name)
        {
            // Get facility_id
            var facility_id = getNodes().Keys.First();

            var p = new Dictionary<string, string>
            {
                {"label", room_name}
            };

            // Get area IDs
            var areas = this.execute(new GET(uri: String.Format("facilities/{0}/areas", facility_id),
                                                        parameters: p));

            var area_ids = new List<string>();
            foreach(var area_data in JArray.Parse(areas.ToString()))
            {
                area_ids.Add(area_data["id"].ToObject<string>());
            }

            // Get sensor IDs
            var sensor_ids = new List<int>();

            foreach(var area_id in area_ids)
            {
                var sensors = this.execute(new GET(uri: String.Format("areas/{0}/sensors", area_id)));

                var sensor_data = JsonConvert.DeserializeObject<PagedResponse<Sensor>>(sensors.ToString());
                
                foreach(var sensor in sensor_data)
                {
                    sensor_ids.Add(sensor.sensor_id);                
                }
            }



            return new Tuple<List<int>, List<String>>(sensor_ids,new List<string>{"Temperature"});


        }
    }
}