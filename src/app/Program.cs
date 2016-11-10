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
            var flywheel = new FlywheelingService();

            SetpointData setpoints = flywheel.getSetPointsForZone(zone_id:"7d5ef0d8-e00b-448b-aa04-59caf22ce91e");

            foreach(KeyValuePair<DateTime, bool> setpoint in setpoints)
            {
                Console.WriteLine("At " + setpoint.Key + ": " + setpoint.Value);
            }


            Dictionary<string, SetpointData> buildingSetpoints = 
                flywheel.getSetPointsForBuilding(building_id:"46445500-6542-4c8c-a250-1093ef17a71f");

            foreach(KeyValuePair<string, SetpointData> building_setpoint in buildingSetpoints)
            {
                Console.WriteLine("Building name" + building_setpoint.Key);

                foreach(KeyValuePair<DateTime, bool> setpoint in building_setpoint.Value)
                {
                    Console.WriteLine("\tAt " + setpoint.Key + ": " + setpoint.Value);
                }
            }


           
        }
    }
}
