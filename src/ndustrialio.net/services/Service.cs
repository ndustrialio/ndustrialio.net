
using System;
using com.ndustrialio.api.http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace com.ndustrialio.api.services
{



    public abstract class Service : APIService
    {
        private string _clientID, _clientSecret;

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

            _clientID = client_id;
            _clientSecret = client_secret;

            getAcessToken();
            
        }

        public abstract string Audience
        {
            get;
        }

        private void getAcessToken()
        {
            Auth0Service service = new Auth0Service();

            _accessToken = service.getAccessToken(_clientID, _clientSecret, this.Audience);
        }

        public override APIResponse execute(APIRequest request)
        {

            try
            {
                return base.execute(request);
            } catch (Exception e)
            {
                // Catch expired JWT token here
                if (checkExpiredJWT(e.Message))
                {
                    // Got expired JWT token, get a new token
                    getAcessToken();

                    // Retry method with new token
                    return base.execute(request);

                } else
                {
                    // Evidently not a JWT expired exception
                    // re-throw
                    throw e;
                }
            }
        }

        private bool checkExpiredJWT(string exception)
        {
            try
            {
                dynamic decoded = JObject.Parse(exception);

                if (decoded.status_code == 401 &&
                    decoded.response_data == "jwt expired")
                {
                    return true;
                } else
                {
                    return false;
                }

            } catch (JsonException e)
            {
                // Not valid JSON, return false
                return false;
            }
        }

    }

}