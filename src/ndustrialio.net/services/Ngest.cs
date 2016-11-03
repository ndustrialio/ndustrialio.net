using System;
using com.ndustrialio.api.http;
 

namespace com.ndustrialio.api.services
{
    public class Ngest : LegacyService
    {

        public Ngest() : base() { }

        public override string baseURL()
        {
            return "https://data.ndustrial.io";
        }

        public void sendData(string feedToken, string feedKey, string data)
        {
            object[] uriChunks = {feedToken, "ngest", feedKey};

            APIRequest req = new POST(uri:String.Join("/", uriChunks),
                body: data)
                .authorize(false); // Ngest requests do not require authorization

            this.execute(req);

        }

    }
}
