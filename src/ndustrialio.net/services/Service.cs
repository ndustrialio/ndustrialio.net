
using com.ndustrialio.api.http;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.ndustrialio.api.services
{



    public abstract class Service : APIService
    {
        public static string AUTH0_URL = "https://ndustrialio.auth0.com";



            //     return self.post(
            // 'https://%s/oauth/token' % self.domain,
            // data={
            //     'client_id': client_id,
            //     'client_secret': client_secret,
            //     'audience': audience,
            //     'grant_type': grant_type
            // },
            // headers={'Content-Type': 'application/json'}



        public Service(string client_id, string client_secret = null)
        {
            // Figure out client secret
            if (client_secret == null)
            {
                client_secret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            }

            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("client_id", client_id);
            body.Add("client_secret", client_secret);
            body.Add("audience", audience());
            body.Add("grant_type", "client_credentials");



            // Need to do a POST to auth0 to get our credentials
            APIRequest credential_request = new POST(uri: "oauth/token", 
                                        body: JsonConvert.SerializeObject(body))
                                        .authorize(false) // Not our API
                                        .baseURL(AUTH0_URL); // Set auth0 url
        }

        public abstract string audience();

    }

}