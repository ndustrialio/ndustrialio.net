using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ndustrialio.api.http;
using Newtonsoft.Json;


namespace com.ndustrialio.api.services
{
    public class TokenData : ApiData
    {
        [JsonProperty("access_token")]
        public String AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public String RefreshToken { get; set; }

    }


    public class Oauth : Service
    {
        public static String URI = "/oauth";

        public Oauth(Client client) : base(client) { }

        public TokenData refreshToken(Dictionary<String, String> args)
        {
            Response response = this._post(new Request(Oauth.URI, body:args));

            TokenData ret = JsonConvert.DeserializeObject<TokenData>(response.ToString());

            return ret;
        }

    }
}
