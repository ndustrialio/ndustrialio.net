
using com.ndustrialio.api.http;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.ndustrialio.api.services
{



    public abstract class Service : APIService
    {

        public Service(string client_id, string client_secret = null)
        {
            // Figure out client secret
            if (client_secret == null)
            {
                client_secret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            }

            // Get 
            Auth0Service service = new Auth0Service();

            _accessToken = service.getAccessToken(client_id, client_secret, audience());
            
        }

        public abstract string audience();

    }

}