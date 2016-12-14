using System;
using System.Collections.Generic;
using com.ndustrialio.api.services;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var feeds = new FeedService();

            dynamic feed = feeds.getFeed(key: "egauge15550");

            string timezone = feed.timezone;
            string key = feed.key;

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
