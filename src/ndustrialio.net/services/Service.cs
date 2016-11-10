
using System;


namespace com.ndustrialio.api.services
{



    public abstract class Service : APIService
    {

        public Service(string client_id = null, string client_secret = null)
        {
            // Figure out client id/secret
            if (client_id == null)
            {
                client_id = Environment.GetEnvironmentVariable("CLIENT_ID");
            }

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