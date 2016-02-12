using System;
using com.ndustrialio.api.http;
 

namespace com.ndustrialio.api.services
{
    public class Ngest : Service
    {
        public static String URL = "https://data.ndustrial.io/v1/";

        public Ngest(Client client) : base(client) { }


        public void sendData(String feedToken, String feedKey, String data)
        {
            Request req = new Request(uri:feedToken + "/ngest" + feedKey, body: data);

            req.BaseURL = Ngest.URL;
            req.ContentType = Request.JSON_CONTENT_TYPE;
            req.Authorize = false; // Ngest requests do not require authorization

            this._post(req);
        }

    }
}
