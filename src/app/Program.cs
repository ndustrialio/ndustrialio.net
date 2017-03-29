using System;
using System.Collections.Generic;
using com.ndustrialio.api.services;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            FlywheelingService service = new FlywheelingService();

            var miraloma_systems = service.getNodes()[25];

            foreach(var system in miraloma_systems)
            {
                Console.WriteLine("System: " + system.SystemID);

                var setpoints = service.getSchemesForSystem(system_id: system.SystemID);

                foreach(KeyValuePair<string, SetpointData> system_setpoint in setpoints.Item2)
                {
                    Console.WriteLine("System name" + system_setpoint.Key);

                    // Setpoints are a simple Dictionary<DateTime, bool>
                    foreach(KeyValuePair<DateTime, string> setpoint in system_setpoint.Value)
                    {
                        Console.WriteLine("\tAt " + setpoint.Key + ": " + setpoint.Value);
                    }
                }

            }
        }
    }
}
