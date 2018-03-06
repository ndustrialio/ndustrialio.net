
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using com.ndustrialio.api.http;



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
            var service = new ContxtAuthService();

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
                    // Reset request so it can run again
                    return base.execute(request.reset());

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
                JObject decoded = JObject.Parse(exception);

                if ((int)decoded["code"] == 401 &&
                    (string)decoded["message"] == "jwt expired")
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