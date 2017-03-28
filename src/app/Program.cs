using System;
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

                var setpoints = service.getSchemesForSystem(system_id: system.SystemID)


            }
        }
    }
}
