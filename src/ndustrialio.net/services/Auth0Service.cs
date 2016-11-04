using System.Collections.Generic;
using com.ndustrialio.api.http;
using Newtonsoft.Json;

namespace com.ndustrialio.api.services
{

    public class Auth0Service : APIService

    {
        public override string baseURL()
        {
            return "https://ndustrialio.auth0.com";
        }

        public string getAccessToken(string client_id, 
                                        string client_secret,
                                        string audience)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("client_id", client_id);
            body.Add("client_secret", client_secret);
            body.Add("audience", audience);
            body.Add("grant_type", "client_credentials");

            // Need to do a POST to auth0 to get our credentials
            APIRequest credential_request = new POST(uri: "oauth/token", 
                                        body: JsonConvert.SerializeObject(body))
                                        .authorize(false) // Not our API, don't authorize
                                        .version(false); // Not our API, don't version

            APIResponse response = execute(credential_request);

            Dictionary<string, string> responseData = 
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(response.ToString());

            // Save token
            string access_token =  responseData["access_token"];

            return access_token;
        }

    }
}

