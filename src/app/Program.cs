using System;
using System.Collections.Generic;
using com.ndustrialio.api.services;
using Newtonsoft.Json.Linq;
using com.ndustrialio.api.ngest;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                Console.WriteLine(z.Id);

            // Instantiate feed service.. in this case 
            // client_id and secret are in the environment
            var feeds = new FeedService();

            // Get feed details by key.. obviously change this to your key

            // Grab feed details
            Feed feed = feeds.getFeed(key: "ngest-test-feed");


            // Good to go!
            var ngest = new NgestClient(feed.Key, feed.Token, feed.Timezone);

            TimeSeriesData data = ngest.newTimeSeries();

            data.addValue(DateTime.Now, "blah", 25);


            ngest.sendDataAsync(data);

            // Initialize Flywheeling services
            // Here you must provide your Client ID and Client Secret somehow.
            // Either environment variable or direct paramter pass.
            // If your code were a person, the Client ID and Secret would be its username and password.
            var flywheel = new FlywheelingService();
            

            // NOTE: any "ID" is a UUID, generally
            // Get your edge node ID and system ID, grouped by facility number in a Dictionary
            // Replace "42" with your facility number (Macon is 75)
            // Edge Node ID is the ID that identifies your development machine (or deployed code at the facility)
            // within the ndustrial.io backend
            // 
            // System ID is the UUID of the refrigeration system.  Your Client ID determins what comes back here, 
            // so there are no arguments. 
            var facility_systems = flywheel.getNodes();

            // Pick the systems for your facility
            var systems = facility_systems[75];

            foreach (var system in systems)
            {

                Console.WriteLine("System: " + system.SystemID);

                // Use the SystemID property to get setpoints.  Only future setpoints
                // Will be returned. 
                Dictionary<string, SetpointData> systemSetpoints = 
                    flywheel.getSetPointsForSystem(system_id:system.SystemID);

                foreach(KeyValuePair<string, SetpointData> system_setpoint in systemSetpoints)
                {
                    Console.WriteLine("System name" + system_setpoint.Key);

                    // Setpoints are a simple Dictionary<DateTime, bool>
                    foreach(KeyValuePair<DateTime, bool> setpoint in system_setpoint.Value)
                    {
                        Console.WriteLine("\tAt " + setpoint.Key + ": " + setpoint.Value);
                    }
                }
            }

        }
    }
}
