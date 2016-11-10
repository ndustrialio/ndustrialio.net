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


            Dictionary<string, SetpointData> systemSetpoints = 
                flywheel.getSetPointsForSystem(system_id:"228fc7a1-f410-409a-8f70-7602a5669a24");

            foreach(KeyValuePair<string, SetpointData> system_setpoint in systemSetpoints)
            {
                Console.WriteLine("System name" + system_setpoint.Key);

                foreach(KeyValuePair<DateTime, bool> setpoint in system_setpoint.Value)
                {
                    Console.WriteLine("\tAt " + setpoint.Key + ": " + setpoint.Value);
                }
            }


           
        }
    }
}
