using System;
using com.ndustrialio.api.http;
using System.Threading.Tasks;
 

namespace com.ndustrialio.api.services
{
    public class NgestService : LegacyService
    {

        public NgestService() : base() { }

        public override string BaseURL
        {
            get{return "https://data.ndustrial.io";}
        }

        public void sendData(string feedToken, string feedKey, string data)
        {
            object[] uriChunks = {feedToken, "ngest", feedKey};

            APIRequest req = new POST(uri:String.Join("/", uriChunks),
                body: data)
                .authorize(false); // Ngest requests do not require authorization

            this.execute(req);

        }

        public async Task<APIResponse> sendDataAsync(string feedToken, string feedKey, string data)
        {
            object[] uriChunks = {feedToken, "ngest", feedKey};

            APIRequest req = new POST(uri:String.Join("/", uriChunks),
                body: data)
                .authorize(false); // Ngest requests do not require authorization

            var response = await this.executeAsync(req);

            return response;
        }

    }
}
