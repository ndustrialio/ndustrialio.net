using com.ndustrialio.api.http;
using System;

namespace com.ndustrialio.api.services
{

    public class LegacyService : APIService
    {
        public LegacyService(string access_token=null)
        {
            if (access_token == null)
            {
                access_token = Environment.GetEnvironmentVariable("ACCESS_TOKEN");
            }

            _client = new Client(access_token);
        }

        public override string baseURL()
        {
            return "https://api.ndustrial.io";
        }

        
    }

}