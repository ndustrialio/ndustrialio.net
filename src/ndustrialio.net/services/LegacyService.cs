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

            _accessToken = access_token;
        }

        public override string BaseURL
        {
            get{return "https://api.ndustrial.io";}
        }

        
    }

}