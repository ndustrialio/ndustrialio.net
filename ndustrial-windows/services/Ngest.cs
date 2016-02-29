using System;
using com.ndustrialio.api.http;
 

namespace com.ndustrialio.api.services
{
    public class Ngest : Service
    {
        public static String URL = "https://data.ndustrial.io";

        public Ngest(Client client) : base(client) { }


        public void sendData(String feedToken, String feedKey, String data)
        {
            Request req = new Request(uri:"/" + feedToken + "/ngest/" + feedKey, 
                body: data, 
                content_type: Request.JSON_CONTENT_TYPE);

            req.BaseURL = Ngest.URL;
            req.Authorize = false; // Ngest requests do not require authorization

            this._post(req);
        }

    }
}
