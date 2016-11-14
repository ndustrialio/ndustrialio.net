using System;
using System.Collections.Generic;
using com.ndustrialio.api.services;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize Flywheeling services
            // var flywheel = new FlywheelingService();

            // var facility_systems = flywheel.getNodes();

            // var systems = facility_systems[42];

            // foreach (var system in systems)
            // {
            //     Console.WriteLine("System: " + system.SystemID);


            //     Dictionary<string, SetpointData> systemSetpoints = 
            //         flywheel.getSetPointsForSystem(system_id:system.SystemID);

            //     foreach(KeyValuePair<string, SetpointData> system_setpoint in systemSetpoints)
            //     {
            //         Console.WriteLine("System name" + system_setpoint.Key);

            //         foreach(KeyValuePair<DateTime, bool> setpoint in system_setpoint.Value)
            //         {
            //             Console.WriteLine("\tAt " + setpoint.Key + ": " + setpoint.Value);
            //         }
            //     }
            // }

            var service = new FeedService();

            while (true)
            {
                dynamic feed = service.getFeed(id:300);
                Console.WriteLine("Feed key: " + feed.key);
                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
